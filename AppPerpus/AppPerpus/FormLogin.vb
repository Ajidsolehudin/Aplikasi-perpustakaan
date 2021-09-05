Imports System.Data.SqlClient
Public Class FormLogin
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call Koneksi()
        Cmd = New SqlCommand("Select * From TBL_PETUGAS where KodePetugas='" & TextBox1.Text & "' and PasswordPetugas='" & TextBox2.Text & "'", Conn)
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Rd.HasRows Then
            Me.Close()
            Call BukaKunci()
            FormMenuUtama.STLabel2.Text = Rd!Kodepetugas
            FormMenuUtama.STLabel4.Text = Rd!Namapetugas
            FormMenuUtama.STLabel6.Text = Rd!Levelpetugas
        Else
            MsgBox("Kode Petugas Atau Password Salah!")
        End If
    End Sub

    Sub BukaKunci()
        FormMenuUtama.LoginToolStripMenuItem.Enabled = False
        FormMenuUtama.LogoutToolStripMenuItem.Enabled = True
        FormMenuUtama.MasterToolStripMenuItem.Enabled = True
        FormMenuUtama.TransaksiToolStripMenuItem.Enabled = True
        FormMenuUtama.UtilityToolStripMenuItem.Enabled = True
        FormMenuUtama.LaporanToolStripMenuItem.Enabled = True
    End Sub
    Private Sub FormLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = "PTG001"
        TextBox2.Text = "ADMIN"
        TextBox2.PasswordChar = "X"
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TextBox1.MaxLength = 6
    End Sub
End Class