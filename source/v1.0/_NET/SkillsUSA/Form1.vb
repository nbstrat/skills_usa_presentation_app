#Region " - header"
'-----------------------------------------------------------------------------------
' XML data application for MGCCC SkillsUSA project
' Copyright 2011 - Shane Walters - ALL RIGHTS RESERVED
'
' Developer Contact: 
' Shane Walters
' 
'
'TERMS OF USE: 
'	  Please note that this is copyrighted work.
'	  You may use it to assist SkillsUSA Mississippi and nothing else!
'	  You may modify anything you want, but my credit tag, copyright statement and 
'	  contact information MUST remain and be prominently displayed at the top of this file.

' ASSISTANCE:
' 	I am fully aware that code like this tends to get handed off to other people in the attempt
'	to make changes and I have had to work on more than my fair-share of hand-me-down projects.
'	If you have inherited this project, please try to comment your work so that the next guy 
'	can pick up where you left off. If you have questions, want some help, etc, 
'	feel free to contact me.
'-----------------------------------------------------------------------------------
'DEVELOPER CHANGE LOG:
' March 11, 2011 - Shane Walters
'   Initial Development Complete! BETA released.
'March 16, 2011
'   Added LEFT OUTER JOIN to sql statement to prevent 'missing' students when the data does not have a corresponding category
'       This will allow loading the student even when the categories were spelled different, missing etc...
'   Added message for missing categories/images on data load
'   Added highlighting to row cells for missing image values - does not check for missing image
'   Added scroll re-positioning variables to maintain scroll position on data update/delete events
'
'March 31, 2011
'   Added IMEX=1 to the extended properties of the connection string. This will allow the Jet driver to handle null / string values
'   in columns such as TEAM. (or at least in this case).
'   
'-----------------------------------------------------------------------------------
#End Region

Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Configuration


Public Class Form1

    Private dataAdapter As New OleDbDataAdapter()
    Private dtSkills As New DataTable
    Private _isDirty As Boolean
    'Private _setControlsOnRecordChange As Boolean
    Private _removeNonAwardedRecordsOnOutput As Boolean


    'todo
    'DONE: Test install on machine without .NET 4 framework - recompiled with NET 3.5 as target framework
    ' Add image file check routine for any possible grid edits of image names.
    ' - decided against this - if this was a daily use app it would require it, but for once a year not worth the code effort.
    'DONE: Add XML output success/fail prompt
    'DONE: Add FLASH runtime XML properties output
    'DONE: Remove the load existing XML option from the file handler
    'DONE: Fix logic controlling grid size to window size.
    'DONE: Add logic for team categories
    'DONE: Add remove all Non-Award records automatically option (add user controls for enabling / disabling)
    'DONE: Add user controls for enabled/disabling SetControlsOnRecordChange behaviour
    'DONE: Add counter displays to assist user in finalizing decision before outputing xml
    'DONE: call SWF from app
    'REMOVED filter issue with bind controls to grid selection when filter is applied (? remove this option ?)


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnGo.Enabled = False
        btnUpdate.Enabled = False
        btnRemoveRow.Enabled = False
        DataGridView1.Visible = False
        gbRadioButtons.Enabled = False
        cbHasPrize.Enabled = False

        FilterSelection.DropDownStyle = ComboBoxStyle.DropDownList
        FilterSelection.Visible = False


        'check .NET version
        'MessageBox.Show(Environment.Version.ToString)

    End Sub

    'load the datagrid with data
    Protected Sub LoadDataGrid(ByVal FileName As String)
        Try
            'set up the public dtSkills data table
            GetDataTable(FileName)

            ' set up the data source
            BindingSource1.DataSource = dtSkills
            BindingSource1.Filter = ""

            ' bind the grid to the source
            DataGridView1.DataSource = BindingSource1

            With DataGridView1

                .AutoGenerateColumns = True
                '.AutoSize = True
                .ScrollBars = ScrollBars.Both
                .AllowUserToResizeColumns = True
                ' Automatically resize the visible rows.
                '.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells

                ' Set the DataGridView control's border.
                .BorderStyle = BorderStyle.Fixed3D

                ' put the cells in edit mode when user enters them or uses the F2 key
                .EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
                ' do not allow row additions
                .AllowUserToAddRows = False
                ' allow row deletes
                .AllowUserToDeleteRows = True

                'resize the grid based on form size
                .Anchor = AnchorStyles.Bottom + AnchorStyles.Top + AnchorStyles.Left

            End With

            Call EnableControls()

            'display row counts
            FilterCount()


            Call HighlightMissingImageCells()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'highlight missing image cells
    Private Sub HighlightMissingImageCells()
        For i As Integer = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(i).Cells("IMAGE").Value Is DBNull.Value Then
                DataGridView1.Rows(i).Cells("IMAGE").Style.BackColor = Color.Red
            End If
        Next
    End Sub

    'export XML click event
    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        DataTableToXMLResponse(BindingSource1.DataSource)
    End Sub

    'create a data table from the Excel file 
    Protected Sub GetDataTable(ByVal FilePath As String) ' As DataTable

        'connection string for use with Excel files
        'note the sytax around the Extended Properties when using multiple properties - IMPORTANT
        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0; Extended Properties='Excel 8.0;IMEX=1';Data Source=" & FilePath & ";"
        'Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0; Extended Properties=Excel 12.0; Data Source=" & FilePath & ";"
        'Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & FilePath & ";"

        ' create connection
        Dim oCn As New OleDbConnection(sConnectionString)

        'MessageBox.Show(oCn.ConnectionString)

        'Create new DataSet to hold infor from the worksheet.
        Dim ds As New DataSet()
        ds.Tables.Add("slides")

        'declare the column data types to prevent data type interpretation issues when reading from Excel
        ' NOTE: This helped, but did not fully correct the issue. The JET driver will read the schema from
        ' the XLS file and determine the data type (usually from the first 8 rows of data). 
        'There is a documented bug with this driver concerning this issue and the override method that
        ' is suppose to prevent the driver from determining the data type.
        ' To make life easy, either use all numberic entries or all alpha-numeric entries for the columns
        ' and don't try to mix the two within the same column.
        With ds.Tables(0).Columns
            .Add(New DataColumn("CATEGORY", System.Type.GetType("System.String")))
            .Add(New DataColumn("SCHOOL", System.Type.GetType("System.String")))
            .Add(New DataColumn("STUDENT", System.Type.GetType("System.String")))
            .Add(New DataColumn("SCHOOL_LEVEL", System.Type.GetType("System.String")))
            .Add(New DataColumn("TEAM", System.Type.GetType("System.String")))
            .Add(New DataColumn("AWARD", System.Type.GetType("System.String")))
            .Add(New DataColumn("PRIZE", System.Type.GetType("System.String")))
            .Add(New DataColumn("IMAGE", System.Type.GetType("System.String")))
        End With

        Try
            ' open the connection
            oCn.Open()

            'Dim sql As String = "SELECT distinct a.CATEGORY, b.SCHOOL, b.FIRST_NAME & ' ' & b.LAST_NAME as STUDENT, b.SCHOOL_LEVEL, b.TEAM, b.AWARD, b.PRIZE, a.IMAGE" _
            '                    + " FROM [Sheet1$] a,  [Sheet2$] b" _
            '                    + " WHERE a.CATEGORY = b.CATEGORY" _
            '                   + " ORDER BY a.CATEGORY, b.SCHOOL, b.SCHOOL_LEVEL, b.TEAM"

            Dim sql As String = "SELECT distinct b.CATEGORY, b.SCHOOL, b.FIRST_NAME & ' ' & b.LAST_NAME as STUDENT, b.SCHOOL_LEVEL, b.TEAM, b.AWARD, b.PRIZE, a.IMAGE" _
                                + " FROM [Sheet2$] b LEFT OUTER JOIN [Sheet1$] a" _
                                + " ON b.CATEGORY = a.CATEGORY" _
                               + " ORDER BY b.CATEGORY, b.SCHOOL, b.SCHOOL_LEVEL, b.TEAM"

            'Dim sql As String = "SELECT distinct [Sheet2$].CATEGORY, [Sheet2$].SCHOOL, [Sheet2$].FIRST_NAME & ' ' & [Sheet2$].LAST_NAME as STUDENT, [Sheet2$].SCHOOL_LEVEL, [Sheet2$].TEAM, [Sheet2$].AWARD, [Sheet2$].PRIZE, [Sheet1$].IMAGE" _
            '                    + " FROM [Sheet2$] LEFT OUTER JOIN [Sheet1$] " _
            '                    + " ON [Sheet2$].CATEGORY = [Sheet1$].CATEGORY" _
            '                   + " ORDER BY [Sheet2$].CATEGORY, [Sheet2$].SCHOOL, [Sheet2$].SCHOOL_LEVEL, [Sheet2$].TEAM"

            'Dim oCmd As OleDbCommand = New OleDbCommand(sql, oCn)
            'Dim oReader As OleDbDataReader = oCmd.ExecuteReader()
            'MessageBox.Show(oReader.HasRows)
            'MessageBox.Show(oReader.RecordsAffected.ToString)
            ''MessageBox.Show(oReader(0).ToString)
            'While oReader.Read
            '    If oReader("CATEGORY") IsNot DBNull.Value Then
            '        If oReader("TEAM") Is DBNull.Value Then
            '            MessageBox.Show(oReader("TEAM").ToString)
            '        Else
            '            MessageBox.Show(oReader("TEAM").ToString)
            '        End If
            '    End If

            'End While




            ' create a data adapter 
            Me.dataAdapter = New OleDbDataAdapter(sql, oCn)
            'Me.dataAdapter.FillLoadOption = LoadOption.OverwriteChanges
            '' fill the dataset
            Me.dataAdapter.Fill(ds, "slides")



            'dev help block
            'For i As Integer = 0 To dtSkills.Rows.Count - 1
            '    MessageBox.Show(dtSkills.Rows(i).Item("TEAM").ToString)
            'Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' close connection 
            oCn.Close()
        End Try

        'assign to the public data table
        dtSkills = ds.Tables(0)

        'create a new ID data column
        Dim id As New DataColumn
        id.ColumnName = "ID"
        ' add the column to the table
        dtSkills.Columns.Add(id)
        'populate the column with index values
        For i As Integer = 0 To dtSkills.Rows.Count - 1
            dtSkills.Rows(i).Item("ID") = i
        Next

        'create a primary key datacolumn array
        Dim pk(1) As DataColumn
        ' add the new ID column to the pk column array
        pk(0) = id
        'set the primary key of the table to the pk array
        dtSkills.PrimaryKey = pk

        'create a data table with a distinct collection of the CATEGORY items
        ' for the FILTER control
        Dim categoryTable As DataTable = dtSkills.DefaultView.ToTable(True, "CATEGORY")
        FilterSelection.DataSource = categoryTable
        FilterSelection.DisplayMember = "CATEGORY"
        FilterSelection.ValueMember = "CATEGORY"



        'check for null IMAGE values
        Dim isMissingImages As Boolean = False
        Dim MissingImageMessage As String = ""
        For i As Integer = 0 To dtSkills.Rows.Count - 1
            If dtSkills.Rows(i).Item("IMAGE") Is DBNull.Value Then
                isMissingImages = True
            End If
        Next
        If isMissingImages = True Then
            MissingImageMessage = "There are missing categories and/or images for the data being used." + vbCrLf + "Please correct and reload this data source before continuing."
            MessageBox.Show(MissingImageMessage)
        End If

    End Sub


    'converts a data table to XML output file
    Protected Sub DataTableToXMLResponse(ByVal dt As DataTable)

        'MessageBox.Show(GetAppPath())
        Try
            'Dim OutputDirectory As String = GetAppPath() + "_data"
            'Dim OutputDirectory As String = "..\.\_data"
            'MessageBox.Show(OutputDirectory)

            ' file name for the xml output file
            Dim FileName As String = "skills_usa_data.xml"

            ' team event logic variables
            Dim IsTeam As Boolean = False
            Dim CurrentTeam As String = ""
            Dim CurrentSchool As String = ""

            'XML compliant character entity for line break
            ' "&#xD;" is the value we need to be XML compliant 
            ' so it can be properly read from the Flash XML reader
            ' "&#xD;" is the equivelant of a char(13), vbCr, /n, etc...
            'the extra spaces are to assist human eyes when reviewing the xml output 
            Dim LineFeed As String = " " & vbCr & " "

            'XML writer settings
            Dim settings As New System.Xml.XmlWriterSettings
            settings.Encoding = System.Text.Encoding.UTF8
            settings.Indent = True
            settings.IndentChars = "    "
            settings.NewLineHandling = System.Xml.NewLineHandling.Entitize
            settings.CloseOutput = True

            settings.ConformanceLevel = System.Xml.ConformanceLevel.Document

            ' Using xml writer to output the file so any null values will be retained
            ' opposed to using DataSet.writexml which will omit null data elements in the output.
            Dim writer As System.Xml.XmlWriter = System.Xml.XmlTextWriter.Create(FileName, settings)
            'Dim writer As System.Xml.XmlWriter = System.Xml.XmlTextWriter.Create(OutputDirectory & "\" & FileName, settings)

            'start xml write...
            writer.WriteStartDocument() 'document spec
            writer.WriteStartElement(dt.TableName.ToLower()) 'root tag

            'write FLASH runtime XML elements
            writer.WriteComment(" flash runtime xml properties ")
            'MessageBox.Show("AppSettings: " & ConfigurationManager.AppSettings.AllKeys.Length.ToString)

            For i As Integer = 0 To ConfigurationManager.AppSettings.Count - 1
                writer.WriteStartElement(ConfigurationManager.AppSettings.Keys(i).ToString)
                writer.WriteString(ConfigurationManager.AppSettings.Item(i).ToString)
                writer.WriteEndElement()
            Next
            writer.WriteComment(" end flash runtime properties ")


            'parse the data table to output the xml elements
            If dt.Rows.Count > 0 Then

                For r As Integer = 0 To dt.Rows.Count - 1

                    Dim row As DataRow = dt.Rows(r)

                    'check Non-Award option / row value conditions
                    If SkipThisRow(row) = False Then

                        writer.WriteStartElement("slide") 'root tag for the row

                        'team logic
                        If row("TEAM").ToString.Length > 0 Then
                            IsTeam = True
                            CurrentTeam = row("TEAM").ToString
                            CurrentSchool = row("SCHOOL").ToString
                            'MessageBox.Show("CurrentTeam: " & CurrentTeam & " CurrentSchool: " & CurrentSchool)
                        Else
                            IsTeam = False
                        End If

                        Dim colCount As Integer = dt.Columns.Count
                        For c As Integer = 0 To colCount - 1
                            'Dim colName As String = ds.Tables(0).Columns(i).ColumnName
                            Dim colName As String = dt.Columns(c).ColumnName.ToLower

                            writer.WriteStartElement(colName) 'child tag for column

                            If row(c).ToString.Length > 0 Then

                                If colName = "student" And IsTeam Then
                                    ' create an array of DataRows to hold the team members
                                    Dim FoundRows() As DataRow
                                    Dim ExpressionFilter As String = "TEAM='" & CurrentTeam & "' AND SCHOOL='" & CurrentSchool & "'"
                                    'MessageBox.Show(ExpressionFilter)
                                    ' use the select method to isolate the student records
                                    FoundRows = dt.Select(ExpressionFilter)

                                    ' Print column 0 of each returned row.
                                    Dim s As String = ""
                                    For i As Integer = 0 To FoundRows.GetUpperBound(0)
                                        'MessageBox.Show(foundRows(i)("STUDENT"))
                                        s += FoundRows(i)("STUDENT")
                                        If i < FoundRows.GetUpperBound(0) Then
                                            s += LineFeed
                                        End If
                                    Next i

                                    'increment the row counter to bypass the student records 
                                    'that were part of the team
                                    r += FoundRows.GetUpperBound(0)

                                    writer.WriteString(s.ToString) ' data for child tag
                                Else
                                    writer.WriteString(row(c).ToString) ' data for child tag
                                End If

                            End If 'If row(c).ToString.Length > 0

                            writer.WriteEndElement() 'ending child tag
                        Next c 'next column in the row

                        'reset IsTeam conditions
                        IsTeam = False
                        'TeamCount = 0

                        'If ds.Tables(0).Rows.Count > 1 Then
                        '    writer.WriteEndElement()
                        'End If

                        If dt.Rows.Count > 1 Then
                            writer.WriteEndElement()
                        End If

                    End If ' If SkipThisRow check

                Next r 'next row in the table
            End If
            writer.WriteEndElement() ' ending root tag
            writer.WriteEndDocument() 'ending doc tag
            writer.Close()
            'Throw New Exception("test exception")
            MessageBox.Show("XML file generated!")
        Catch ex As Exception
            MessageBox.Show("An error occured while creating the xml output." & vbCrLf & ex.Message.ToString)
        End Try

    End Sub

    'logic for skipping non award/prize records when option is enabled
    Private Function SkipThisRow(ByVal dr As DataRow) As Boolean
        Dim retValue As Boolean = False
        If RemoveNonAwardedRecordsOnOutput = True Then
            Dim awardLength As Integer = dr("AWARD").ToString.Length
            Dim prizeLength As Integer = dr("PRIZE").ToString.Length
            If awardLength < 1 And prizeLength < 1 Then
                retValue = True
            End If
        End If
        Return retValue
    End Function

    'return the application path
    Public Function GetAppPath() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory()
        'Return System.AppDomain.CurrentDomain.
    End Function

#Region " - MenuItems"

    'loads the xls spreadsheet 
    Private Sub menuLoadCategories_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuToolsLoadCategories.Click
        'Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        'openFileDialog1.InitialDirectory = "c:\"
        'openFileDialog1.Filter = "xls files (*.xls)|*.xls|xml files (*.xml)|*.xml"
        openFileDialog1.Filter = "xls files (*.xls)|*.xls"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                'myStream = openFileDialog1.OpenFile()
                'MessageBox.Show(openFileDialog1.FileName.ToString)
                If openFileDialog1.CheckFileExists Then
                    LoadDataGrid(openFileDialog1.FileName.ToString)
                End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' do nothing
            End Try
        End If
    End Sub

    ''toggles the property value on menu selection - removed this option
    'Private Sub BindAwardControlsToRecordToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    SetControlsOnRecordChange = BindAwardControlsToRecordToolStripMenuItem.Checked
    'End Sub

    'toggles the property value on menu selection
    Private Sub RemoveNonAwardRecordsOnOutputToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveNonAwardRecordsOnOutputToolStripMenuItem.Click
        RemoveNonAwardedRecordsOnOutput = RemoveNonAwardRecordsOnOutputToolStripMenuItem.Checked
    End Sub

#End Region

    'TODO - NOT - placeholder for unused image check routine
    Private Sub DataGridView1_CurrentCellDirtyStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged
        'MessageBox.Show("currentCell DirtyState Changed")

        'set IsDirtyState
        IsDirty = True



        'TODO: add AWARD, PRIZE check
        'If = "AWARD1" OR "AWARD2", etc....
        'If Prize = "true"
        'If IMAGE, run check to make sure image name is in the images directory....
        'accept any other change
    End Sub

#Region " - award/prize selections"

    Private Sub UpdateAward()
        Dim AwardValue As String = ""

        Dim iPos As Integer = BindingSource1.Position

        Dim id As Integer = DataGridView1.Rows(iPos).Cells("ID").FormattedValue
        'Dim dgPos As Integer = DataGridView1.SelectedRows.Item(0).Index

        'Dim dr As DataRow = dtSkills.Rows(id)
        Dim dr As DataRow = dtSkills.Rows.Find(id)

        'award radio buttons
        If RadioButton1.Checked Then
            AwardValue = "Award1"
        ElseIf RadioButton2.Checked Then
            AwardValue = "Award2"
        ElseIf RadioButton3.Checked Then
            AwardValue = "Award3"
        ElseIf RadioButton3.Checked Then
            AwardValue = ""
        End If

        dr.Item("AWARD") = AwardValue

        'Prize checkbox
        If cbHasPrize.Checked Then
            dr.Item("PRIZE") = "true"
        Else
            dr.Item("PRIZE") = ""
        End If

        dtSkills.AcceptChanges()

        'set focus back on the selected row so user doesn't have to scroll again to their previous position
        DataGridView1.Rows(iPos).Cells(0).Selected = True

    End Sub

    
#End Region

    'update data table record with award level / prize 
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim FirstDisplayedRow As Integer = DataGridView1.FirstDisplayedScrollingRowIndex
        Call UpdateAward()
        DataGridView1.FirstDisplayedScrollingRowIndex = FirstDisplayedRow
    End Sub

    'removes the selected row(s) from the data table
    Private Sub btnRemoveRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveRow.Click
        'MessageBox.Show(BindingSource1.Position)
        'remove the current record from the BindingSource (OK)
        Dim FirstDisplayedRow As Integer = DataGridView1.FirstDisplayedScrollingRowIndex


        BindingSource1.RemoveCurrent()

        dtSkills.AcceptChanges()

        Call HighlightMissingImageCells()

        DataGridView1.FirstDisplayedScrollingRowIndex = FirstDisplayedRow

    End Sub

    'rowleave event - not being used
    Private Sub DataGridView1_RowLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.RowLeave
        If IsDirty = True Then
            'dtSkills.RejectChanges()
            IsDirty = False
        End If
    End Sub

#Region " - properties"

    ' datagrid save/change state
    Public Property IsDirty() As Boolean
        Get
            Return _isDirty
        End Get
        Set(ByVal Value As Boolean)
            _isDirty = Value
        End Set
    End Property

    ''controls behaviour of radio buttons/check box on record change
    ''true - controls will set to the value of the current record
    ''false - controls will maintain state of previous settings
    'Public Property SetControlsOnRecordChange() As Boolean
    '    Get
    '        Return _setControlsOnRecordChange
    '    End Get

    '    Set(ByVal Value As Boolean)
    '        _setControlsOnRecordChange = Value
    '    End Set
    'End Property

    'true - record without award or prize will not be included in XML output
    'false - record without award or prize will be included in XML output
    Public Property RemoveNonAwardedRecordsOnOutput() As Boolean
        Get
            Return _removeNonAwardedRecordsOnOutput
        End Get
        Set(ByVal Value As Boolean)
            _removeNonAwardedRecordsOnOutput = Value
        End Set
    End Property
#End Region

    Private Sub EnableControls()
        btnGo.Enabled = True
        btnUpdate.Enabled = True
        btnRemoveRow.Enabled = True
        DataGridView1.Visible = True
        gbRadioButtons.Enabled = True
        cbHasPrize.Enabled = True

        FilterSelection.Visible = True
        FilterSelection.Enabled = False

        'user controlled options
        'controls behaviour of radio buttons/check box on record change
        'SetControlsOnRecordChange = False
        'omit non award or prize records on xml output
        RemoveNonAwardedRecordsOnOutput = False
        'check mark display for menu items indicating to the user what they have enabled
        'BindAwardControlsToRecordToolStripMenuItem.CheckOnClick = True
        RemoveNonAwardRecordsOnOutputToolStripMenuItem.CheckOnClick = True

    End Sub

    'controls behaviour of radio buttons/check box on record change - no longer used
    Private Sub DataGridView1_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.RowEnter
        ''SetControlsOnRecordChange is enabled/disabled by user 
        'If SetControlsOnRecordChange And cbFilter.Checked = False Then
        '    'set the value of the radio/check box controls based on the current row data
        '    Dim dr As DataRow = dtSkills.Rows(e.RowIndex)
        '    Dim AwardLevel = dr.Item("AWARD").ToString.ToLower
        '    Dim Prize = dr.Item("PRIZE").ToString.ToLower

        '    If AwardLevel = "award1" Then
        '        RadioButton1.Checked = True
        '    ElseIf AwardLevel = "award2" Then
        '        RadioButton2.Checked = True
        '    ElseIf AwardLevel = "award3" Then
        '        RadioButton3.Checked = True
        '    Else
        '        RadioButton4.Checked = True
        '    End If

        '    If Prize = "true" Then
        '        cbHasPrize.Checked = True
        '    Else
        '        cbHasPrize.Checked = False
        '    End If
        'End If

    End Sub

    'delete row event
    Private Sub DataGridView1_UserDeletedRow(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView1.UserDeletedRow
        dtSkills.AcceptChanges()
    End Sub

#Region " - binding source filter"
    'apply filter selection to data grid
    Private Sub FilterSelection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterSelection.SelectedIndexChanged
        If cbFilter.Enabled Then
            FilterBindingSource()
        End If
    End Sub

    Private Sub cbFilter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilter.CheckedChanged
        If cbFilter.Checked Then
            FilterSelection.Enabled = True
            Call FilterBindingSource()
        Else
            FilterSelection.Enabled = False
            BindingSource1.Filter = ""
        End If
        'display row counts
        FilterCount()
    End Sub

    Private Sub FilterBindingSource()
        BindingSource1.Filter = "CATEGORY='" & FilterSelection.SelectedValue.ToString & "'"
        'update the row count display
        FilterCount()
    End Sub
#End Region

#Region " - helper labels"
    'displayes the row counts
    Private Sub FilterCount()
        lblRowCount.Text = DataGridView1.RowCount & " of " & dtSkills.Rows.Count & " rows"
    End Sub
#End Region

    
    'play the compiled SWF file (Projector EXE version of SWF)
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim pi As New System.Diagnostics.ProcessStartInfo()
            pi.FileName = "SkillsUSA_Presentation.exe"
            pi.WindowStyle = ProcessWindowStyle.Maximized
            Dim p As New System.Diagnostics.Process()
            Process.Start(pi)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

End Class
