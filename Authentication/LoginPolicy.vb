Imports System
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraNEO

Namespace AgoraNEO
    Public Class LoginPolicy
        Dim strMessage As String
        Dim objDb As New EAD.DBCom

        'Public Sub New(Optional ByVal pConnStr As String = "")
        '    If pConnStr <> "" Then
        '        Dim objDb As New EAD.DBCom(
        '    End If
        'End Sub

        Public Property Message() As String
            Get
                Message = strMessage
            End Get
            Set(ByVal Value As String)
                strMessage = Value
            End Set
        End Property

        'Name       : getParamValue
        'Author     : kk
        'Date       : 22/11/2004
        'Descption  : Get Policy value
        Public Function getParamValue(ByVal pParam As String, Optional ByVal pCond As String = "") As String
            getParamValue = "ON"
            Dim lsSql As String = ""

            If pCond <> "" Then
                pCond = " AND " & pCond
            End If

            lsSql = "Select LP_VALUE from LOGIN_POLICY WHERE LP_PARAM='" & pParam & "'" & pCond
            Dim tDS As DataSet

            Try
                tDS = objDb.FillDs(lsSql)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    getParamValue = tDS.Tables(0).Rows(j).Item("LP_VALUE")
                Next
            Catch e As Exception
                Return Nothing
            Finally

            End Try
        End Function

        'Name       : getParamValue
        'Author     : kk
        'Date       : 22/11/2004
        'Descption  : Get Policy value
        Public Function getParamInd(ByVal pParam As String, Optional ByVal pCond As String = "") As String
            getParamInd = "ON"
            Dim lsSql As String = ""

            If pCond <> "" Then
                pCond = " AND " & pCond
            End If

            lsSql = "Select LP_PARAM_IND from LOGIN_POLICY WHERE LP_PARAM='" & pParam & "'" & pCond


            Try
                Dim tDS As DataSet = objDb.FillDs(lsSql)
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    getParamInd = tDS.Tables(0).Rows(j).Item("LP_PARAM_IND")
                Next
            Catch e As Exception
                Return Nothing
            Finally

            End Try
        End Function

        'Name       : SearchPolicy
        'Author     : kk3
        'Date       : 22/11/2004
        'Descption  : Get all policy
        Public Function SearchPolicy() As DataSet
            Dim strSQL As String = "Select * from Login_policy order by LP_AUTO_NO"
            Dim dsPolicy As DataSet
            dsPolicy = objDb.FillDs(strSQL)
            SearchPolicy = dsPolicy

        End Function

        'Name       : UpdatePolicy
        'Author     : kk3
        'Date       : 28/12/2004
        'Descption  : Update pilicy
        Public Function UpdatePolicy(ByVal pPolicy As String, ByVal pInd As String, ByVal pVal As String) As Boolean
            Dim strSQL, Query(0) As String

            Dim objDb As New EAD.DBCom
            strSQL = "UPDATE LOGIN_POLICY SET LP_VALUE='" & pVal & "' WHERE LP_PARAM='" & pPolicy & "'"
            Common.Insert2Ary(Query, strSQL)

            strSQL = "UPDATE LOGIN_POLICY SET LP_PARAM_IND='" & pInd & "' WHERE LP_PARAM='" & pPolicy & "' AND LP_PARAM_IND<>'FIXED'"
            Common.Insert2Ary(Query, strSQL)

            If objDb.BatchExecute(Query) Then
                strMessage = Common.RecordSave
                Return True
            Else
                strMessage = Common.RecordNotSave
                Return False
            End If

        End Function


    End Class
End Namespace



