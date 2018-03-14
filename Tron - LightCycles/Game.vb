Option Strict Off
Option Explicit On
Public Class Game


#Region "Globals"
    Private p1 As PictureBox
    Private p2 As PictureBox
    'Private BlueBikePic As PictureBox
    'Private RedBikePic As PictureBox
    Public tmr1 As New Timer
    Public playing As Boolean
    'Public pvp As Boolean
    Private p1trail As New List(Of Point)
    Private p2trail As New List(Of Point)
    Private bothtrail As New List(Of Point)
    Private p1Direct As PlayerDirection
    Private p2Direct As PlayerDirection
    Public blueScore As Integer = 0
    Public redScore As Integer = 0
    Public blueLives As Integer = 3
    Public redLives As Integer = 3
    Private AITravelDistance As Integer
    Private AIDirect As Integer
    Private AITrailTurn As Integer
    Public start As Boolean
    Private gamePaused As Boolean = False

    Dim PauseForm As New Pause()



    'Public resources As Object = My.Resources

    Public Enum PlayerDirection
        Up
        Down
        Left
        Right
    End Enum
#End Region

#Region "Game Events"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        start = True

        StartGame()

    End Sub

    Private Sub StartGame()

        'If playing = False AndAlso e.KeyCode = Keys.Space Then
        '    playing = True
        '    tmr1.Start()
        'End If

        If start And Not playing Then
            playing = True
            Me.Show()
            tmr1.Start()
        End If

        Game_Load()

    End Sub

    Private Sub KeyPressed(sender As Object, ByVal e As KeyPressEventArgs) Handles MyBase.KeyPress
        If (e.KeyChar = "w") And Not p1Direct = PlayerDirection.Down Then
            p1Direct = PlayerDirection.Up
        ElseIf (e.KeyChar = "a") And Not p1Direct = PlayerDirection.Right Then
            p1Direct = PlayerDirection.Left
        ElseIf (e.KeyChar = "s") And Not p1Direct = PlayerDirection.Up Then
            p1Direct = PlayerDirection.Down
        ElseIf (e.KeyChar = "d") And Not p1Direct = PlayerDirection.Left Then
            p1Direct = PlayerDirection.Right
        End If
        If (e.KeyChar = "i") And Not p2Direct = PlayerDirection.Down Then
            p2Direct = PlayerDirection.Up
        ElseIf (e.KeyChar = "j") And Not p2Direct = PlayerDirection.Right Then
            p2Direct = PlayerDirection.Left
        ElseIf (e.KeyChar = "k") And Not p2Direct = PlayerDirection.Up Then
            p2Direct = PlayerDirection.Down
        ElseIf (e.KeyChar = "l") And Not p2Direct = PlayerDirection.Left Then
            p2Direct = PlayerDirection.Right
        End If
        If PauseForm.Paused = False Then
            If (e.KeyChar = "p") Then
                PauseForm.Paused = True
                'Dim PauseForm As New Pause(redScore, blueScore)
                'tmr1.Stop()
                PauseForm.PausedRedScore = redScore
                PauseForm.PausedBlueScore = blueScore
                PauseForm.Show()
            End If
        Else
            If (e.KeyChar = "p") Then
                PauseForm.Paused = False
                PauseForm.Hide()
            End If
        End If
    End Sub

    Private Sub Game_Load()

        With Me
            .BackColor = Color.Black
            .BackgroundImage = My.Resources.grid
            .BackgroundImageLayout = ImageLayout.Stretch
            .DoubleBuffered = True
            .KeyPreview = True
            .MaximizeBox = False
            .Text = "Tron"
            .WindowState = FormWindowState.Maximized
        End With

        tmr1.Interval = 20

        'pvp = True

        AddHandler tmr1.Tick, AddressOf tmr1_Tick
        Threading.Thread.Sleep(2000)
        Call newGame()
    End Sub

#End Region

#Region "New/End Game"

    Private Sub newGame()
        Me.Controls.Remove(p1)
        Me.Controls.Remove(p2)
        p1 = New PictureBox
        p2 = New PictureBox
        'BlueBikePic = New PictureBox
        'RedBikePic = New PictureBox
        playing = False
        p1trail.Clear()
        p2trail.Clear()
        bothtrail.Clear()
        p1Direct = PlayerDirection.Right
        p2Direct = PlayerDirection.Left


        Refresh()

        With p1
            .BackColor = Color.Blue
            '.BackgroundImage = Image.FromFile("E:\austin\corcework 02032016\Tron\Tron - LightCycles\bike blue.png")
            '.BackgroundImageLayout = ImageLayout.Stretch
            .Location = New Point(CInt(Me.Width / 2 - 10 - Me.Width / 5), CInt(Me.Height / 2))
            .Size = New Size(2, 2) 'for pic use (50, 20)
        End With

        With p2
            .BackColor = Color.Red
            '.BackgroundImage = Image.FromFile("E:\austin\corcework 02032016\Tron\Tron - LightCycles\bike red.png")
            '.BackgroundImageLayout = ImageLayout.Stretch
            .Location = New Point(CInt(Me.Width / 2 + 10 + Me.Width / 5), CInt(Me.Height / 2))
            .Size = New Size(2, 2) 'for pic use (50, 20)

        End With

        'With BlueBikePic
        '    .BackColor = Color.Blue
        '    '.BackgroundImage = resources.GetObject("bike blue.png")
        '    .BackgroundImage = Image.FromFile("F:\austin\corcework CURRENT\Tron\Tron - LightCycles\bike blue.png")
        '    .BackgroundImageLayout = ImageLayout.Stretch
        '    .Size = New Size(50, 20)
        '    .Location = p1.Location
        'End With

        'With RedBikePic
        '    .BackColor = Color.Red
        '    '.BackgroundImage = resources.GetObject("bike red.png")
        '    .BackgroundImage = Image.FromFile("F:\austin\corcework CURRENT\Tron\Tron - LightCycles\bike red.png")
        '    .BackgroundImageLayout = ImageLayout.Stretch
        '    .Size = New Size(50, 20)
        '    .Location = p2.Location
        'End With

        p1trail.Add(p1.Location)
        p2trail.Add(p2.Location)
        bothtrail.AddRange({p1.Location, p2.Location})
        'BlueBikePic.Location = p1.Location
        'RedBikePic.Location = p2.Location

        Controls.AddRange({p1, p2})

        Refresh()
    End Sub

    Private Sub checkEndGame()
        Dim endGame As Boolean = False

        For Each trail As Point In bothtrail
            Dim rect As New Rectangle(trail, New Size(CInt(1.5), CInt(1.5)))
            If p1.Bounds.IntersectsWith(rect) AndAlso p1trail.Count > 1 Then
                tmr1.Stop()
                redScore = redScore + 1
                blueLives = 3 - redScore
                endGame = True
                If redLives = 0 And blueScore = 3 Then
                    MessageBox.Show("Team blue is the winner with a score of: " & blueScore)
                    Menu1.Show()
                    Me.Close()
                    Exit For
                ElseIf blueLives = 0 And redScore = 3 Then
                    MessageBox.Show("Team red is the winner with a score of: " & redScore)
                    Menu1.Show()
                    Me.Close()
                    Exit For
                Else
                    MessageBox.Show("At the end of this round: " & vbCrLf & "Team blue is on: " & blueLives & " lives and Team red is on: " & redLives & " lives." & vbCrLf & "Team blue has a score of: " & blueScore & " and Team red has a score of: " & redScore)
                End If
                start = True
                StartGame()
                Exit For
            ElseIf p2.Bounds.IntersectsWith(rect) AndAlso p2trail.Count > 1 Then
                tmr1.Stop()
                blueScore = blueScore + 1
                redLives = 3 - blueScore
                endGame = True
                If redLives = 0 And blueScore = 3 Then
                    MessageBox.Show("Team blue is the winner with a score of: " & blueScore)
                    Menu1.Show()
                    Me.Close()
                    Exit For
                ElseIf blueLives = 0 And redScore = 3 Then
                    MessageBox.Show("Team red is the winner with a score of: " & redScore)
                    Menu1.Show()
                    Me.Close()
                    Exit For
                Else
                    MessageBox.Show("At the end of this round: " & vbCrLf & "Team blue is on: " & blueLives & " lives and Team red is on: " & redLives & " lives." & vbCrLf & "Team blue has a score of: " & blueScore & " and Team red has a score of: " & redScore)
                End If
                start = True
                StartGame()
                Exit For
            End If
        Next

        If p1.Left < 0 OrElse p1.Top < 0 OrElse p1.Left > Me.Width OrElse p1.Top > Me.Height Then
            tmr1.Stop()
            redScore = redScore + 1
            blueLives = 3 - redScore
            endGame = True
            If redLives = 0 And blueScore = 3 Then
                MessageBox.Show("Team blue is the winner with a score of: " & blueScore)
                Menu1.Show()
                Me.Close()
            ElseIf blueLives = 0 And redScore = 3 Then
                MessageBox.Show("Team red is the winner with a score of: " & redScore)
                Menu1.Show()
                Me.Close()
            Else
                MessageBox.Show("At the end of this round: " & vbCrLf & "Team blue is on : " & blueLives & " lives and Team red is on: " & redLives & " lives." & vbCrLf & "Team blue has a score of: " & blueScore & " and Team red has a score of: " & redScore)
            End If
            start = True
            StartGame()
        ElseIf p2.Left < 0 OrElse p2.Top < 0 OrElse p2.Left > Me.Width OrElse p2.Top > Me.Height Then
            tmr1.Stop()
            blueScore = blueScore + 1
            redLives = 3 - blueScore
            endGame = True
            If redLives = 0 And blueScore = 3 Then
                MessageBox.Show("Team blue is the winner with a score of: " & blueScore)
                Menu1.Show()
                Me.Close()
            ElseIf blueLives = 0 And redScore = 3 Then
                MessageBox.Show("Team red is the winner with a score of: " & redScore)
                Menu1.Show()
                Me.Close()
            Else
                MessageBox.Show("At the end of this round: " & vbCrLf & "Team blue is on: " & blueLives & " lives and Team red is on: " & redLives & " lives." & vbCrLf & "Team blue has a score of: " & blueScore & " and Team red has a score of: " & redScore)
            End If
            start = True
            StartGame()
        End If

        If endGame Then
            Call newGame()
        End If
    End Sub

#End Region

#Region "Painting"

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Me.Paint
        Dim g As Graphics = e.Graphics

        For Each trail As Point In p1trail
            Dim rect As New Rectangle(trail, New Size(2, 2))
            g.FillRectangle(New SolidBrush(Color.Blue), rect)
        Next

        For Each trail As Point In p2trail
            Dim rect As New Rectangle(trail, New Size(2, 2))
            g.FillRectangle(New SolidBrush(Color.Red), rect)
        Next

    End Sub

#End Region

#Region "Timers"



    Private Sub changeAIDirection()
        Dim newP2Trail As New Point()
        Dim isValidMove As Boolean = True
        AIDirect = CInt(Math.Floor((2 - 1 + 1) * Rnd())) + 1
        If p2Direct = PlayerDirection.Left Then
            If AIDirect = 1 Then
                Call TestDirect(newP2Trail, PlayerDirection.Up, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Up
                Else
                    p2Direct = PlayerDirection.Down
                End If
            ElseIf AIDirect = 2 Then
                Call TestDirect(newP2Trail, PlayerDirection.Down, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Down
                Else
                    p2Direct = PlayerDirection.Up
                End If
            End If
        ElseIf p2Direct = PlayerDirection.Down Then
            If AIDirect = 1 Then
                Call TestDirect(newP2Trail, PlayerDirection.Left, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Left
                Else
                    p2Direct = PlayerDirection.Right
                End If
            ElseIf AIDirect = 2 Then
                Call TestDirect(newP2Trail, PlayerDirection.Right, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Right
                Else
                    p2Direct = PlayerDirection.Left
                End If
            End If
        ElseIf p2Direct = PlayerDirection.Right Then
            If AIDirect = 1 Then
                Call TestDirect(newP2Trail, PlayerDirection.Up, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Up
                Else
                    p2Direct = PlayerDirection.Down
                End If
            ElseIf AIDirect = 2 Then
                Call TestDirect(newP2Trail, PlayerDirection.Up, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Down
                Else
                    p2Direct = PlayerDirection.Up
                End If
            End If
        ElseIf p2Direct = PlayerDirection.Up Then
            If AIDirect = 1 Then
                Call TestDirect(newP2Trail, PlayerDirection.Left, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Left
                Else
                    p2Direct = PlayerDirection.Right
                End If
            ElseIf AIDirect = 2 Then
                Call TestDirect(newP2Trail, PlayerDirection.Right, isValidMove)
                If isValidMove Then
                    p2Direct = PlayerDirection.Right
                Else
                    p2Direct = PlayerDirection.Left
                End If
            End If
        End If
    End Sub

    Public Sub TestDirect(ByRef point As Point, ByVal direction As PlayerDirection, ByRef isValid As Boolean)
        If direction = PlayerDirection.Up Then
            point.X = p2.Location.X
            point.Y = p2.Location.Y - 2
        ElseIf direction = PlayerDirection.Down Then
            point.X = p2.Location.X
            point.Y = p2.Location.Y + 2
        ElseIf direction = PlayerDirection.Left Then
            point.X = p2.Location.X - 2
            point.Y = p2.Location.Y
        ElseIf direction = PlayerDirection.Right Then
            point.X = p2.Location.X + 2
            point.Y = p2.Location.Y
        End If

        If bothtrail.Contains(point) Then
            isValid = False
        ElseIf point.X < 50 AndAlso p2trail.Count > 1 Then
            isValid = False
        ElseIf point.X > Me.Width - 50 AndAlso p2trail.Count > 1 Then
            isValid = False
        ElseIf point.Y < 50 AndAlso p2trail.Count > 1 Then
            isValid = False
        ElseIf point.Y > Me.Height - 50 AndAlso p2trail.Count > 1 Then
            isValid = False
        End If
    End Sub

    Private Sub tmr1_Tick(ByVal sender As Object, ByVal e As EventArgs)


        If PauseForm.QuitGame = True Then
            Me.Close()
        ElseIf PauseForm.Paused = True Then
            'tmr1.Stop()
            Return
        Else
            PauseForm.Hide()
            Select Case p1Direct
                Case PlayerDirection.Left
                    p1.Left -= 2
                Case PlayerDirection.Down
                    p1.Top += 2
                Case PlayerDirection.Right
                    p1.Left += 2
                Case PlayerDirection.Up
                    p1.Top -= 2
            End Select

            'While (True)
            '    BlueBikePic.Location = p1.Location
            '    RedBikePic.Location = p2.Location
            'End While

            'pvp = True
            If Settings.playervplayer = True Then
                Select Case p2Direct
                    Case PlayerDirection.Left
                        p2.Left -= 2
                    Case PlayerDirection.Down
                        p2.Top += 2
                    Case PlayerDirection.Right
                        p2.Left += 2
                    Case PlayerDirection.Up
                        p2.Top -= 2
                End Select
            ElseIf Settings.playervplayer = False Then
                'BlueBikePic.Location = p1.Location
                'RedBikePic.Location = p2.Location
                AITravelDistance = CInt(Math.Floor((20 - 1 + 1) * Rnd())) + 1
                If p2Direct = PlayerDirection.Left Then
                    p2.Left -= 2
                ElseIf p2Direct = PlayerDirection.Down Then
                    p2.Top += 2
                ElseIf p2Direct = PlayerDirection.Right Then
                    p2.Left += 2
                ElseIf p2Direct = PlayerDirection.Up Then
                    p2.Top -= 2
                End If

                Dim newP2Trail As New Point()
                Dim isValidMove As Boolean = True
                Call TestDirect(newP2Trail, p2Direct, isValidMove)

                If isValidMove = False Then
                    Call changeAIDirection()
                End If

                'If bothtrail.Contains(newP2Trail) Then
                '    Call changeAIDirection()
                'ElseIf newP2Trail.X < 50 OrElse newP2Trail.X > Me.width - 50 OrElse newP2Trail.y < 50 OrElse newP2Trail.y > Me.height - 50 AndAlso p2trail.Count > 1 Then
                '    Call changeAIDirection()
                'End If

            End If

            Call checkEndGame()

            p1trail.Add(p1.Location)
            p2trail.Add(p2.Location)
            bothtrail.AddRange({p1.Location, p2.Location})
            'BlueBikePic.Location = p1.Location
            'RedBikePic.Location = p2.Location
        End If
    End Sub

#End Region



End Class

'Sections of code sourced from: http://goo.gl/oHZ5t4