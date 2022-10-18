﻿Imports DiscUtils.Iso9660
Imports System.IO
Imports System.Net
Imports System.Reflection.Emit
Imports System.Text
Imports HtmlAgilityPack

Public Class ExtractPS2
    Shared Sub info(isopath As String)
        Try
            Using psISO As FileStream = File.Open(isopath, FileMode.Open)
                Dim dvd As New CDReader(psISO, True)
                Dim info As Stream = dvd.OpenFile("SYSTEM.CNF", FileMode.Open)
                Dim convinfo As New StreamReader(info, Encoding.UTF8)
                File.WriteAllText("ps2.info", convinfo.ReadToEnd)
            End Using

            getid(File.ReadAllText("ps2.info"))
            getv(File.ReadAllText("ps2.info"))
            getr(File.ReadAllText("ps2.info"))
            MsgBox(Application.StartupPath & "\bin\info")
            File.Move("ps2.info", Application.StartupPath & "\bin\info\" & Form1.Label5.Text.Replace("Game ID: ", "") & ".info")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "PS22PS4-GUI")
        End Try

    End Sub

    Shared Sub getid(str)
        Try
            Dim sSource As String = str 'String that is being searched
            Dim sDelimStart As String = "cdrom0:\" 'First delimiting word
            Dim sDelimEnd As String = ";" 'Second delimiting word
            Dim nIndexStart As Integer = sSource.IndexOf(sDelimStart) 'Find the first occurrence of f1
            Dim nIndexEnd As Integer = sSource.IndexOf(sDelimEnd) 'Find the first occurrence of f2

            If nIndexStart > -1 AndAlso nIndexEnd > -1 Then '-1 means the word was not found.
                Dim res As String = Strings.Mid(sSource, nIndexStart + sDelimStart.Length + 1, nIndexEnd - nIndexStart - sDelimStart.Length) 'Crop the text between
                'MessageBox.Show(res.Replace("_", "").Replace(".", "")) 'Display
                Form1.Label5.Text = "Game ID: " & res.Replace("_", "").Replace(".", "")
                getn(res.Replace("_", "-").Replace(".", ""))
            Else
                MessageBox.Show("One or both of the delimiting words were not found!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "PS22PS4-GUI")
        End Try

    End Sub

    Shared Sub getv(str)
        Try
            Dim sSource As String = str 'String that is being searched
            Dim sDelimStart As String = "VER = " 'First delimiting word
            Dim sDelimEnd As String = "VMODE" 'Second delimiting word
            Dim nIndexStart As Integer = sSource.IndexOf(sDelimStart) 'Find the first occurrence of f1
            Dim nIndexEnd As Integer = sSource.IndexOf(sDelimEnd) 'Find the first occurrence of f2

            If nIndexStart > -1 AndAlso nIndexEnd > -1 Then '-1 means the word was not found.
                Dim res As String = Strings.Mid(sSource, nIndexStart + sDelimStart.Length + 1, nIndexEnd - nIndexStart - sDelimStart.Length) 'Crop the text between
                'MessageBox.Show(res) 'Display
                Form1.Label6.Text = "Game version: " & res
            Else
                MessageBox.Show("One or both of the delimiting words were not found!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "PS22PS4-GUI")
        End Try

    End Sub

    Shared Sub getr(str)
        Dim sSource As String = str 'String that is being searched
        Dim res As String = sSource.Substring(sSource.LastIndexOf("VMODE = ") + 7) 'Crop the text between
        'MessageBox.Show(res) 'Display
        Form1.Label7.Text = "Game Region: " & res
    End Sub

    Shared Sub getn(id As String)
        Try
            Dim wc As New WebClient()
            Dim html = wc.DownloadString("http://redump.org/discs/quicksearch/" & id)
            wc.Dispose()
            Dim htmlDoc As New HtmlDocument()
            htmlDoc.LoadHtml(html)
            For Each h1Node In htmlDoc.DocumentNode.SelectNodes("//h1")
                ' Do Something...
                Form1.Label8.Text = "Game name: " & h1Node.InnerHtml
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "PS22PS4-GUI")
        End Try


    End Sub
End Class