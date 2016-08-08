Public Class AtomizeForm
    Private Sub AtomizeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.BackgroundImage = CreatAtomizeBitmap(My.Resources.AtomizeResource.AtomizeTest, 0)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Static TestBitmap As Bitmap = My.Resources.AtomizeResource.AtomizeTest
        TestBitmap = CreatWaveAtomize(TestBitmap, 50)
        'TestBitmap = CreatWaveBlur(TestBitmap, 60)
        PictureBox1.Image = TestBitmap
    End Sub

    Private Function CreatWaveBlur(ByVal OriginalBitmap As Bitmap, ByVal OriginalRadius As Long) As Bitmap
        Dim BitmapSize As Size = OriginalBitmap.Size
        VBMath.Randomize()
        Dim BlurBitmap As Bitmap = OriginalBitmap
        Dim OriginalPoint As Point = New Point(VBMath.Rnd * (BitmapSize.Width - 1), VBMath.Rnd * (BitmapSize.Height - 1))
        Dim Distance As Integer, BlurRadius As Integer
        Dim Count As Integer, CenterColor As Color
        Dim SumR, SumG, SumB As Long
        For Y As Integer = OriginalPoint.Y - OriginalRadius To OriginalPoint.Y + OriginalRadius
            For X As Integer = OriginalPoint.X - OriginalRadius To OriginalPoint.X + OriginalRadius
                If (0 <= X And X < BitmapSize.Width) And (0 <= Y And Y < BitmapSize.Height) Then
                    Distance = Math.Sqrt((X - OriginalPoint.X) ^ 2 + (Y - OriginalPoint.Y) ^ 2)
                    If (Distance < OriginalRadius) Then '圆形区域
                        Count = 0 : SumR = 0 : SumG = 0 : SumB = 0
                        BlurRadius = OriginalRadius - Distance
                        For RoundY = Y - BlurRadius To Y + BlurRadius
                            For RoundX = X - BlurRadius To X + BlurRadius
                                If (0 <= RoundX And RoundX < BitmapSize.Width) And (0 <= RoundY And RoundY < BitmapSize.Height) Then
                                    Count += 1
                                    SumR += OriginalBitmap.GetPixel(RoundX, RoundY).R
                                    SumG += OriginalBitmap.GetPixel(RoundX, RoundY).G
                                    SumB += OriginalBitmap.GetPixel(RoundX, RoundY).B
                                End If
                            Next
                        Next
                        CenterColor = Color.FromArgb(255, SumR \ Count, SumG \ Count, SumB \ Count)
                        BlurBitmap.SetPixel(X, Y, CenterColor)
                    End If
                End If
            Next
        Next

        Return BlurBitmap
    End Function

    Private Function CreatWaveAtomize(ByVal OriginalBitmap As Bitmap, ByVal OriginalRadius As Integer) As Bitmap
        Dim BitmapSize As Size = OriginalBitmap.Size
        VBMath.Randomize()
        Dim AtomizeBitmap As Bitmap = OriginalBitmap
        Dim OriginalPoint As Point = New Point(VBMath.Rnd * (BitmapSize.Width - 1), VBMath.Rnd * (BitmapSize.Height - 1))
        Dim Distance As Integer, AtomizeRadius As Integer
        Dim RndMin, RndMax, GetPixelPoint As Point
        Dim TColor As Color
        For Y As Integer = OriginalPoint.Y - OriginalRadius To OriginalPoint.Y + OriginalRadius
            For X As Integer = OriginalPoint.X - OriginalRadius To OriginalPoint.X + OriginalRadius
                If (0 <= X And X < BitmapSize.Width) And (0 <= Y And Y < BitmapSize.Height) Then
                    Distance = Math.Sqrt((X - OriginalPoint.X) ^ 2 + (Y - OriginalPoint.Y) ^ 2)
                    If (Distance < OriginalRadius) Then '圆形区域
                        AtomizeRadius = OriginalRadius - Distance
                        RndMin.X = IIf(X - AtomizeRadius > 0, AtomizeRadius, X)
                        RndMin.Y = IIf(Y - AtomizeRadius > 0, AtomizeRadius, Y)
                        RndMax.X = IIf(X + AtomizeRadius < BitmapSize.Width - 1, AtomizeRadius, BitmapSize.Width - X - 1)
                        RndMax.Y = IIf(Y + AtomizeRadius < BitmapSize.Height - 1, AtomizeRadius, BitmapSize.Height - Y - 1)
                        VBMath.Randomize()
                        GetPixelPoint.X = X - RndMin.X + VBMath.Rnd() * (RndMin.X + RndMax.X)
                        GetPixelPoint.Y = Y - RndMin.Y + VBMath.Rnd() * (RndMin.Y + RndMax.Y)
                        TColor = OriginalBitmap.GetPixel(GetPixelPoint.X, GetPixelPoint.Y)
                        AtomizeBitmap.SetPixel(X, Y, TColor)
                    End If
                End If
            Next
        Next

        Return AtomizeBitmap
    End Function

    Private Function CreatAtomizeBitmap(ByVal OriginalBitmap As Bitmap, ByVal AtomizeRadius As Integer) As Bitmap
        Dim BitmapSize As Size = OriginalBitmap.Size
        Dim AtomizeBitmap As Bitmap = New Bitmap(BitmapSize.Width, BitmapSize.Height)
        Dim RndMin, RndMax, GetPixelPoint As Point
        For X As Integer = 0 To BitmapSize.Width - 1
            For Y As Integer = 0 To BitmapSize.Height - 1
                RndMin.X = IIf(X - AtomizeRadius > 0, AtomizeRadius, X)
                RndMin.Y = IIf(Y - AtomizeRadius > 0, AtomizeRadius, Y)
                RndMax.X = IIf(X + AtomizeRadius < BitmapSize.Width - 1, AtomizeRadius, BitmapSize.Width - X - 1)
                RndMax.Y = IIf(Y + AtomizeRadius < BitmapSize.Height - 1, AtomizeRadius, BitmapSize.Height - Y - 1)
                '以上是为了在图像雾化半径超越图像边缘时依然能正确取随机坐标
                GetPixelPoint.X = X - RndMin.X + VBMath.Rnd() * (RndMin.X + RndMax.X)
                GetPixelPoint.Y = Y - RndMin.Y + VBMath.Rnd() * (RndMin.Y + RndMax.Y)
                AtomizeBitmap.SetPixel(X, Y, OriginalBitmap.GetPixel(GetPixelPoint.X, GetPixelPoint.Y))
            Next
        Next

        Return AtomizeBitmap
    End Function

End Class
