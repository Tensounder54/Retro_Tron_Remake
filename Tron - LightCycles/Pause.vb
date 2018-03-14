Public Class Pause

    Public Paused As Boolean
    Public PausedRedScore As Integer
    Public PausedBlueScore As Integer
    Public QuitGame As Boolean = False

    Private Sub desplayRedScore() Handles Me.VisibleChanged
        Label5.Text = CStr(PausedRedScore)
        Label6.Text = CStr(PausedBlueScore)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PausedSettings.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim result As DialogResult = MessageBox.Show("Are you want to quit to the menu?", Me.Text, MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Menu1.Show()
            QuitGame = True
            Me.Close()
        End If
    End Sub

    Private Sub UnPause_Click(sender As Object, e As EventArgs) Handles UnPause.Click
        Paused = False
    End Sub
End Class