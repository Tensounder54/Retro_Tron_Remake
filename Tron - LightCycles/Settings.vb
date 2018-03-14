Option Strict Off

Imports System.Runtime.InteropServices
Imports System.IO

Public Class Settings
    Public playervplayer As Boolean = False
    Private valid1 As Boolean


    'Api to send the commands to the mci device.
    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" _
    (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal _
    uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer

    'Private Sub Qpaused()
    '    If Pause.Paused = True Then
    '        Label3.Visible = False
    '        Button4.Visible = False
    '        ReturnToMenu.Text = "Return to Pause Menu"
    '    ElseIf Pause.Paused = False Then
    '        Label3.Visible = True
    '        Button4.Visible = True
    '        ReturnToMenu.Text = "Return to Menu"   
    '    End If
    'End Sub

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function
    Const WM_APPCOMMAND As UInteger = &H319
    Const APPCOMMAND_VOLUME_UP As UInteger = &HA
    Const APPCOMMAND_VOLUME_DOWN As UInteger = &H9
    Const APPCOMMAND_VOLUME_MUTE As UInteger = &H8
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SendMessage(Me.Handle, WM_APPCOMMAND, &H30292, APPCOMMAND_VOLUME_UP * &H10000)
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SendMessage(Me.Handle, WM_APPCOMMAND, &H30292, APPCOMMAND_VOLUME_DOWN * &H10000)
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Button3.Text = "Mute" Then
            Button3.Text = "Un-Mute"
            SendMessage(Me.Handle, WM_APPCOMMAND, &H200EB0, APPCOMMAND_VOLUME_MUTE * &H10000)
        ElseIf Button3.Text = "Un-Mute" Then
            Button3.Text = "Mute"
            SendMessage(Me.Handle, WM_APPCOMMAND, &H200EB0, APPCOMMAND_VOLUME_MUTE * &H10000)
        End If
    End Sub
    'Code Sourced form: https://goo.gl/yfxISd

    Private Sub ReturnToMenu_Click(sender As Object, e As EventArgs) Handles ReturnToMenu.Click
        Menu1.Show()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button4.Text = "OFF" Then
            Button4.Text = "ON"
            playervplayer = True
        ElseIf Button4.Text = "ON" Then
            Button4.Text = "OFF"
            playervplayer = False
        End If

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)  'And textBox1.mouseleave

        'Dim myStream As Stream = Nothing
        'Dim openFileDialog1 As New OpenFileDialog()
        'Dim filepath As String

        'openFileDialog1.InitialDirectory = "c:\"
        'openFileDialog1.Filter = "MP3 files (*.MP3)|*.MP3|All files (*.*)|*.*"
        'openFileDialog1.Filter = "wav files (*.wav)|*.wav|All files (*.*)|*.*"
        'openFileDialog1.Filter = "WMA files (*.WMA)|*.WMA|All files (*.*)|*.*"
        'openFileDialog1.FilterIndex = 2
        'openFileDialog1.RestoreDirectory = True

        'If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
        '    Try
        '        myStream = openFileDialog1.OpenFile()
        '        If (myStream IsNot Nothing) Then
        '            filepath = openFileDialog1.FileNames
        '        End If
        '    Catch Ex As Exception
        '        MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
        '    Finally
        '        ' Check this again, since we need to make sure we didn't throw an exception on open.
        '        If (myStream IsNot Nothing) Then
        '            myStream.Close()
        '        End If
        '    End Try
        'End If

        ''Dim filepath As String

        ''Dim ItemToBeSerched As String
        ''Dim Locate As String
        ''Dim Position As Integer

        ''ItemToBeSerched = "me@me.com"
        ''Locate = "@"

        ''Position = InStr(ItemToBeSerched, Locate)

        ''Dim openFileDialog As New OpenFileDialog()
        ''If openFileDialog.ShowDialog() = DialogResult.OK Then
        ''    Try
        ''        filepath = openFileDialog.FileName


        ''    Catch ex As Exception

        ''    End Try
        ''End If

        ''The Chr(34) code is to put quotes at the beginning and end of the file's path.
        ''You can convert the long filename to the short filename and not need 'the quotes.
        ''That would just be more code that really isn't needed.

        'Dim fileToPlay As String

        'fileToPlay = Chr(34) & filepath & Chr(34)

        ''Let the command interface decide which device to use. Just specify the alias.
        ''The alias is the name you use to program that device. You can create multiple
        ''devices to play media at the same time and such with different alias's.
        'mciSendString("open " & fileToPlay & " alias myDevice", vbNullString, 0, 0)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        mciSendString("play customAlias", vbNullString, 0, 0)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Button4.Text = "Pause" Then
            Button4.Text = "Resume"
            mciSendString("pause myDevice", vbNullString, 0, 0)
        ElseIf Button4.Text = "Resume" Then
            Button4.Text = "Pause"
            mciSendString("resume myDevice", vbNullString, 0, 0)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        mciSendString("stop myDevice", vbNullString, 0, 0)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "e:\"
        openFileDialog1.Filter = "MP3 files (*.MP3)|*.MP3|wav files (*.wav)|*.wav|WMA files (*.WMA)|*.WMA"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        Dim fileToPlay As String = String.Empty

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                myStream = openFileDialog1.OpenFile()
                If (myStream IsNot Nothing) Then
                    fileToPlay = Chr(34) & openFileDialog1.FileName & Chr(34) '*
                    mciSendString("open " & fileToPlay & " alias myDevice", vbNullString, 0, 0) '**
                End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open.
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If

        '*
        'The Chr(34) code is to put quotes at the beginning and end of the file's path.
        'You can convert the long filename to the short filename and not need 'the quotes.
        'That would just be more code that really isn't needed.

        '**
        'Let the command interface decide which device to use. Just specify the alias.
        'The alias is the name you use to program that device. You can create multiple
        'devices to play media at the same time and such with different alias's.


    End Sub

    'Sections of code sourced from: http://goo.gl/nn2IoG & http://goo.gl/HcMWMh

End Class
