Imports System

Namespace AgoraNEO
    Public Class InputException
        Inherits ApplicationException
        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub
        Public Sub New(ByVal msg As String, ByVal inner As Exception)
            MyBase.New(msg, inner)
        End Sub
    End Class

    Public Class PRNotCreatedException
        Inherits ApplicationException
        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub
        Public Sub New(ByVal msg As String, ByVal inner As Exception)
            MyBase.New(msg, inner)
        End Sub
    End Class


    Public Class PONotCreatedException
        Inherits ApplicationException
        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub
        Public Sub New(ByVal msg As String, ByVal inner As Exception)
            MyBase.New(msg, inner)
        End Sub
    End Class

    Public Class CustomException
        Inherits ApplicationException
        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub
        Public Sub New(ByVal msg As String, ByVal inner As Exception)
            MyBase.New(msg, inner)
        End Sub
    End Class
End Namespace