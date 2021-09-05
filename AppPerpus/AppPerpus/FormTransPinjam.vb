Imports System.Data.SqlClient
Public Class FormTransPinjam
    Sub KondisiAwal()
        Call NoOtomatis()
        Call MunculKodeAnggota()
        LBLNamaPetugas.Text = FormMenuUtama.STLabel4.Text
        LBLNama.Text = ""
        LBLAlamat.Text = ""
        LBLTelepon.Text = ""
        LBLJudul.Text = ""
        LBLPengarang.Text = ""
        LBLTahun.Text = ""
        LBLTotalBuku.Text = "0"
        ComboBox1.Text = ""
        Call BuatKolom()
    End Sub
    Private Sub FormTransPinjam_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call KondisiAwal()
        TextBox1.MaxLength = 6
        LBLTanggal.Text = Today
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        LBLJam.Text = TimeOfDay

    End Sub
    Sub NoOtomatis()
        Call Koneksi()
        Cmd = New SqlCommand("Select * from TBL_PINJAM  where NoPinjam in (select max(NoPinjam) from TBL_PINJAM)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "PJM" + Format(Now, "yyMMdd") + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 9) + 1
            UrutanKode = "PJM" + Format(Now, "yyMMdd") + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        LBLNoPinjam.Text = UrutanKode
    End Sub

    Sub MunculKodeAnggota()
        Call Koneksi()
        ComboBox1.Items.Clear()
        Cmd = New SqlCommand("Select * from tbl_anggota", Conn)
        Rd = Cmd.ExecuteReader
        Do While Rd.Read
            ComboBox1.Items.Add(Rd.Item(0))
        Loop
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Call Koneksi()
        Cmd = New SqlCommand("Select * from tbl_anggota where kodeanggota ='" & ComboBox1.Text & "'", Conn)
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Rd.HasRows Then
            LBLNama.Text = Rd!NamaAnggota
            LBLAlamat.Text = Rd!AlamatAnggota
            LBLTelepon.Text = Rd!TelpAnggota
        End If
    End Sub
    Sub BuatKolom()
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("Kode", "Kode Buku")
        DataGridView1.Columns.Add("Judul", "Judul Buku")
        DataGridView1.Columns.Add("Pengarang", "Pengarang")
        DataGridView1.Columns.Add("TahunBuku", "TahunBuku")
        DataGridView1.Columns.Add("Jumlah", "Jumlah")
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New SqlCommand("Select * from tbl_buku where kodebuku='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Rd.HasRows Then
                TextBox1.Text = Rd.Item("KodeBuku")
                LBLJudul.Text = Rd.Item("JudulBuku")
                LBLPengarang.Text = Rd.Item("PengarangBuku")
                LBLTahun.Text = Rd.Item("TahunBuku")
                TextBox2.Enabled = True
                TextBox2.Text = "1"
            Else
                MsgBox("Kode Buku Tidak Ada")
            End If
        End If
    End Sub
    Sub RumusTotalBuku()
        Dim HitungItem As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            HitungItem = HitungItem + DataGridView1.Rows(i).Cells(4).Value
            LBLTotalBuku.Text = HitungItem
        Next
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If LBLTotalBuku.Text >= 5 Or Val(LBLTotalBuku.Text) + Val(TextBox2.Text) > 5 Then
            MsgBox("Buku yang dipinjam melebihi maksimal!")
        Else
            If LBLJudul.Text = "" Or TextBox2.Text = "" Then
                MsgBox("Silahkan masukkan kode buku dan tekan ENTER!")
            Else
                DataGridView1.Rows.Add(New String() {TextBox1.Text, LBLJudul.Text, LBLPengarang.Text, LBLTahun.Text, TextBox2.Text})
                TextBox1.Text = ""
                TextBox2.Text = ""
                LBLJudul.Text = ""
                TextBox2.Text = ""
                LBLPengarang.Text = ""
                LBLTahun.Text = ""
                Call RumusTotalBuku()
            End If
        End If
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Call KondisiAwal()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If LBLNama.Text = "" Or Label9.Text = "" Then
            MsgBox("Transaksi tidak ada, silahkan lakukan transaksi terlebih dahulu")
        Else
            Call Koneksi()
            Dim tglsql As String
            tglsql = Format(Today, "yyyy-MM-dd")
            Dim PinjamBuku As String = "Insert into tbl_pinjam values ('" & LBLNoPinjam.Text & "','" & tglsql & "','" & LBLJam.Text & "','" & ComboBox1.Text & "','" & LBLTotalBuku.Text & "','" & LBLTotalBuku.Text & "','" & FormMenuUtama.STLabel2.Text & "')"
            Cmd = New SqlCommand(PinjamBuku, Conn)
            Cmd.ExecuteNonQuery()

            For Baris As Integer = 0 To DataGridView1.Rows.Count - 2
                Call Koneksi()
                Dim SimpanDetail As String = "Insert into tbl_DetailPinjam values('" & LBLNoPinjam.Text & "','" & DataGridView1.Rows(Baris).Cells(0).Value & "','" & DataGridView1.Rows(Baris).Cells(1).Value & "','" & DataGridView1.Rows(Baris).Cells(4).Value & "')"
                Cmd = New SqlCommand(SimpanDetail, Conn)
                Cmd.ExecuteNonQuery()
                Call Koneksi()
                Cmd = New SqlCommand("select * from tbl_buku where kodebuku='" & DataGridView1.Rows(Baris).Cells(0).Value & "'", Conn)
                Rd = Cmd.ExecuteReader
                Rd.Read()
                Call Koneksi()
                Dim KurangiStok As String = "Update tbl_buku set JumlahBuku = '" & Rd.Item("JumlahBuku") - DataGridView1.Rows(Baris).Cells(4).Value & "' where KodeBuku='" & DataGridView1.Rows(Baris).Cells(0).Value & "'"
                Cmd = New SqlCommand(KurangiStok, Conn)
                Cmd.ExecuteNonQuery()
            Next
            Call KondisiAwal()
            MsgBox("Transaksi telah berhasil disimpan")
        End If
    End Sub
End Class