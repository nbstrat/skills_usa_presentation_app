<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuToolsLoadCategories = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveNonAwardRecordsOnOutputToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.gbRadioButtons = New System.Windows.Forms.GroupBox()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.cbHasPrize = New System.Windows.Forms.CheckBox()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnRemoveRow = New System.Windows.Forms.Button()
        Me.lblDev = New System.Windows.Forms.Label()
        Me.FilterSelection = New System.Windows.Forms.ComboBox()
        Me.cbFilter = New System.Windows.Forms.CheckBox()
        Me.lblRowCount = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.gbRadioButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(16, 450)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(107, 23)
        Me.btnGo.TabIndex = 0
        Me.btnGo.Text = "Export To Flash"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(134, 31)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(900, 482)
        Me.DataGridView1.TabIndex = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolsToolStripMenuItem, Me.OptionsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1069, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuToolsLoadCategories})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.ToolsToolStripMenuItem.Text = "File"
        '
        'menuToolsLoadCategories
        '
        Me.menuToolsLoadCategories.Name = "menuToolsLoadCategories"
        Me.menuToolsLoadCategories.Size = New System.Drawing.Size(179, 22)
        Me.menuToolsLoadCategories.Text = "Load Data From File"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveNonAwardRecordsOnOutputToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'RemoveNonAwardRecordsOnOutputToolStripMenuItem
        '
        Me.RemoveNonAwardRecordsOnOutputToolStripMenuItem.Name = "RemoveNonAwardRecordsOnOutputToolStripMenuItem"
        Me.RemoveNonAwardRecordsOnOutputToolStripMenuItem.Size = New System.Drawing.Size(287, 22)
        Me.RemoveNonAwardRecordsOnOutputToolStripMenuItem.Text = "Remove Non-Award Records On Output"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(6, 19)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(39, 17)
        Me.RadioButton1.TabIndex = 3
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "1st"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'gbRadioButtons
        '
        Me.gbRadioButtons.Controls.Add(Me.RadioButton4)
        Me.gbRadioButtons.Controls.Add(Me.RadioButton3)
        Me.gbRadioButtons.Controls.Add(Me.RadioButton2)
        Me.gbRadioButtons.Controls.Add(Me.RadioButton1)
        Me.gbRadioButtons.Controls.Add(Me.cbHasPrize)
        Me.gbRadioButtons.Controls.Add(Me.btnUpdate)
        Me.gbRadioButtons.Location = New System.Drawing.Point(9, 105)
        Me.gbRadioButtons.Name = "gbRadioButtons"
        Me.gbRadioButtons.Size = New System.Drawing.Size(115, 193)
        Me.gbRadioButtons.TabIndex = 4
        Me.gbRadioButtons.TabStop = False
        Me.gbRadioButtons.Text = "Awards"
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(7, 91)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(49, 17)
        Me.RadioButton4.TabIndex = 6
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "none"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(7, 67)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(40, 17)
        Me.RadioButton3.TabIndex = 5
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "3rd"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(6, 43)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(43, 17)
        Me.RadioButton2.TabIndex = 4
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "2nd"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'cbHasPrize
        '
        Me.cbHasPrize.AutoSize = True
        Me.cbHasPrize.Location = New System.Drawing.Point(7, 124)
        Me.cbHasPrize.Name = "cbHasPrize"
        Me.cbHasPrize.Size = New System.Drawing.Size(71, 17)
        Me.cbHasPrize.TabIndex = 5
        Me.cbHasPrize.Text = "Has Prize"
        Me.cbHasPrize.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(7, 164)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(103, 23)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "Apply Changes"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnRemoveRow
        '
        Me.btnRemoveRow.Location = New System.Drawing.Point(15, 48)
        Me.btnRemoveRow.Name = "btnRemoveRow"
        Me.btnRemoveRow.Size = New System.Drawing.Size(103, 23)
        Me.btnRemoveRow.TabIndex = 7
        Me.btnRemoveRow.Text = "Remove Record"
        Me.btnRemoveRow.UseVisualStyleBackColor = True
        '
        'lblDev
        '
        Me.lblDev.AutoSize = True
        Me.lblDev.Location = New System.Drawing.Point(771, 10)
        Me.lblDev.Name = "lblDev"
        Me.lblDev.Size = New System.Drawing.Size(37, 13)
        Me.lblDev.TabIndex = 8
        Me.lblDev.Text = "lblDev"
        '
        'FilterSelection
        '
        Me.FilterSelection.FormattingEnabled = True
        Me.FilterSelection.Location = New System.Drawing.Point(223, 4)
        Me.FilterSelection.MaxDropDownItems = 50
        Me.FilterSelection.Name = "FilterSelection"
        Me.FilterSelection.Size = New System.Drawing.Size(209, 21)
        Me.FilterSelection.TabIndex = 0
        '
        'cbFilter
        '
        Me.cbFilter.AutoSize = True
        Me.cbFilter.Location = New System.Drawing.Point(169, 5)
        Me.cbFilter.Name = "cbFilter"
        Me.cbFilter.Size = New System.Drawing.Size(48, 17)
        Me.cbFilter.TabIndex = 10
        Me.cbFilter.Text = "Filter"
        Me.cbFilter.UseVisualStyleBackColor = True
        '
        'lblRowCount
        '
        Me.lblRowCount.AutoSize = True
        Me.lblRowCount.Location = New System.Drawing.Point(456, 9)
        Me.lblRowCount.Name = "lblRowCount"
        Me.lblRowCount.Size = New System.Drawing.Size(65, 13)
        Me.lblRowCount.TabIndex = 11
        Me.lblRowCount.Text = "lbRowCount"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(16, 490)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(107, 23)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "Run Presentation"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1069, 545)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblRowCount)
        Me.Controls.Add(Me.cbFilter)
        Me.Controls.Add(Me.FilterSelection)
        Me.Controls.Add(Me.lblDev)
        Me.Controls.Add(Me.btnRemoveRow)
        Me.Controls.Add(Me.gbRadioButtons)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Skills USA Presentation"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.gbRadioButtons.ResumeLayout(False)
        Me.gbRadioButtons.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents BindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuToolsLoadCategories As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents gbRadioButtons As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents cbHasPrize As System.Windows.Forms.CheckBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnRemoveRow As System.Windows.Forms.Button
    Friend WithEvents lblDev As System.Windows.Forms.Label
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents FilterSelection As System.Windows.Forms.ComboBox
    Friend WithEvents cbFilter As System.Windows.Forms.CheckBox
    Friend WithEvents lblRowCount As System.Windows.Forms.Label
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveNonAwardRecordsOnOutputToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
