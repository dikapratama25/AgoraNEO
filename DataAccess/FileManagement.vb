'//File System
'//Directory.CreateDirectory - error if directory is a map drive
'//File.Copy- error if directory is a map drive
Imports System
Imports System.Web
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Configuration

Namespace AgoraNEO
    Public Enum EnumUploadType
        DocAttachment
        ProductImage
        ProductImagePath
        ProductAttachment
        ProductDocPath
        TermAndCond
        CoyLogo
        PDFDownload
        UserAttachment 'Michelle (eBiz/303) - to allow AO to attach file
        DOAttachment 'Michelle (17/1/2013) - Issue 1727
        DocAttachmentTemp
        CompDocAttachment
    End Enum
    Public Enum EnumDownLoadType
        DocAttachment
        ProductImage
        ProductAttach
        TermAndCond
        CoyLogo
        PO_TermAndCond
        UserAttachment 'Michelle (eBiz/303) - to allow AO to attach file
        DOAttachment 'Michelle (17/1/2013) - Issue 1727
        CompDocAttachment
    End Enum

    Public Enum EnumUploadFrom 'value same as DownloadFrom
        FrontOff
        BackOff
    End Enum


    Public Class FileManagement
        Dim objDb As New EAD.DBCom
        Dim objUsr As New Users

        'Michelle (24/1/2011) - To track the value
        Function WriteLog(ByVal pField As String, ByVal pValue As String)
            Dim LogPath As String = ConfigurationSettings.AppSettings("LogPath")
            Dim LogFile As String = LogPath & "Log_" & Today.Year.ToString & Date.Today.ToString("MM") & Date.Today.ToString("dd") & ".txt"
            Dim objstreemLog As StreamWriter = New StreamWriter(LogFile, True)

            objstreemLog.Write(pField & " - " & pValue)
            objstreemLog.WriteLine()
            objstreemLog.Close()
        End Function

        'SAM (29/4/2011)
        Function FileExist(ByVal strFileName As String, Optional ByVal strCoy As String = "", Optional ByVal strConnStr As String = Nothing) As Boolean
            Dim strSql, strMsg, strBasePath, strFolderPath, strFilePath, strCoyID As String
            Dim strTempBasePath, strMovePath As String

            If strCoy = "" Then
                strCoyID = HttpContext.Current.Session("CompanyId")
            Else
                strCoyID = strCoy
            End If

            strBasePath = getBasePath(0, strConnStr)
            strFolderPath = getSystemParam("ProductImagePath", "", strConnStr)
            strFilePath = strBasePath & strFolderPath & strCoyID & "\" & strFileName

            If File.Exists(strFilePath) Then
                Return True
            Else
                Return False
            End If

        End Function

        '//pUploadType  - image/Doc
        '//strUploadFrom  - Hubadmin(Backoff)/Ehub(FrontOff)
        Function FileUpload(ByVal input As Object, ByVal userID As String, ByVal companyID As String, ByVal hubFile As String, ByVal pEnumUploadType As EnumUploadType,
        ByVal strDocType As String, ByVal pEnumUploadFrom As EnumUploadFrom, ByVal strDocNo As String, Optional ByVal blnTemp As Boolean = False, Optional ByVal strIndex As String = "", Optional ByVal strConnStr As String = Nothing,
         Optional ByVal seq As String = "", Optional ByVal pFrontOfficeSite As String = "", Optional ByVal AttchType As String = "E", Optional ByVal ItemCode As String = "", Optional ByVal LineNo As String = "", Optional ByVal POLine As String = "") As String

            Dim filename As String = String.Empty
            Dim contentType As String = String.Empty
            Dim contentLength As String = String.Empty
            Dim blnAdd As Boolean = True
            Dim strLatest As String
            If input IsNot Nothing Then
                filename = input.CDA_ATTACH_FILENAME
                contentType = input.Type
                contentLength = input.CDA_FILESIZE        'in bytes
                contentLength = contentLength / 1024          'convert to KB
            End If
            'If Not pBrowseFile Is Nothing Then
            '    myFile = pBrowseFile.PostedFile
            '    filename = Path.GetFileName(myFile.FileName)
            '    contentType = myFile.ContentType
            '    '"application/msword"
            '    '"text/plain"
            '    '"application/vnd.ms-excel"
            '    '"image/gif"
            '    '"image/pjpeg"

            '    contentLength = myFile.ContentLength        'in bytes
            '    contentLength = contentLength / 1024          'convert to KB
            'End If


            Dim strSql, strMsg, strBasePath, strUploadPath, strFilePath, strCoyID, strHubFile As String
            Dim strTempBasePath, strMovePath As String

            If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                strCoyID = companyID
            ElseIf Not String.IsNullOrEmpty(companyID) Then
                strCoyID = companyID
            Else
                strCoyID = objUsr.getCompanyID(userID)
            End If

            If Not strConnStr Is Nothing Then
                objDb = New EAD.DBCom(strConnStr)
            End If

            ' testing only
            'strCoyID = HttpContext.Current.Session("CompanyId")

            strBasePath = getBasePath(pEnumUploadFrom, strConnStr)
            ' WriteLog("strBasePath", strBasePath)
            strHubFile = If(Not String.IsNullOrEmpty(hubFile), hubFile, String.Format("{0}_{1}", strDocNo, filename))  'getLastDocAttachFileName(filename, strConnStr)
            ' WriteLog("strHubFile", strHubFile)

            Select Case pEnumUploadType
                Case EnumUploadType.DocAttachment
                    If contentType = "application/msword" Then

                    End If
                    'Michelle (28/12/2010) - To cater for Quotation attachment
                    If strDocType <> "QuotTemp" Then
                        strUploadPath = getSystemParam("DocAttachPath", strDocType, strConnStr)
                    Else
                        strUploadPath = getSystemParam("DocAttachPath", "Quotation", strConnStr)
                    End If
                    strFilePath = strBasePath & strUploadPath               '& strCoyID & "\"
                    '//update Company_Doc_Attachment
                    '//maybe need to check dup before insert
                    strSql = "INSERT INTO Company_Doc_Attachment(CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE," _
                    & "CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE) VALUES('" & strCoyID & "','" &
                    strDocNo & "','" & strDocType & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ",'" & AttchType & "')"
                    '//return query
                Case EnumUploadType.DocAttachmentTemp
                    If contentType = "application/msword" Then

                    End If
                    'Michelle (28/12/2010) - To cater for Quotation attachment
                    If strDocType <> "QuotTemp" Then
                        strUploadPath = getSystemParam("DocAttachPath", strDocType, strConnStr)
                    Else
                        strUploadPath = getSystemParam("DocAttachPath", "Quotation", strConnStr)
                    End If
                    strFilePath = strBasePath & strUploadPath               '& strCoyID & "\"
                    '//update Company_Doc_Attachment
                    '//maybe need to check dup before insert
                    strSql = "INSERT INTO Company_Doc_Attachment_Temp(CDA_COY_ID,CDA_DOC_NO,CDA_DOC_TYPE," _
                    & "CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,CDA_STATUS) VALUES('" & strCoyID & "','" &
                    strDocNo & "','" & strDocType & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ",'" & AttchType & "','N')"
                    '//return query
                Case EnumUploadType.ProductImage
                    '//Remark BY Moo
                    strUploadPath = getSystemParam("ProductImagePath", "", strConnStr)
                    If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                        strTempBasePath = getSystemParam("TempBaseUploadPath", System.Enum.GetName(GetType(EnumUploadFrom), pEnumUploadFrom), strConnStr)
                        'strFilePath = strTempBasePath & strUploadPath
                        strFilePath = strTempBasePath & strUploadPath & strCoyID & "\"
                    Else
                        strFilePath = strBasePath & strUploadPath & strCoyID & "\"
                    End If
                    'getFilePath(pEnumUploadFrom, EnumUploadType.ProductImagePath, strFilePath, strMovePath)
                    '//update Product_Mstr
                    If blnTemp Then             ' insert into temp table
                        Dim strSource As String
                        If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                            strSource = "H"
                        Else
                            'Michelle (8/11/2010) - To cater for BIM
                            'strSource = "V"
                            If pFrontOfficeSite = "B" Then
                                strSource = "B"
                            Else
                                strSource = "V"
                            End If

                        End If
                        'Michelle (8/5/2011)
                        'If objDb.Exist("SELECT '*' FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_SOURCE = '" & strSource & "' ") > 0 Then
                        If objDb.Exist("SELECT '*' FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & strDocNo & "' AND PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_SOURCE = '" & strSource & "' ") > 0 Then
                            blnAdd = False
                            strSql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET "
                            strSql &= "PA_HUB_FILENAME = '" & strHubFile & "', "
                            strSql &= "PA_ATTACH_FILENAME = '" & Common.Parse(filename) & "', "
                            ' ai chu add on 20/09/2005
                            ' set PA_STATUS from 'D' to 'T'
                            strSql &= "PA_STATUS = 'T', "
                            strSql &= "PA_FILESIZE = " & contentLength & " "
                            strSql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' "
                            'Michelle (8/11/2010) - To cater for BIM
                            If pFrontOfficeSite = "B" Then
                                strSql &= "AND PA_SOURCE = 'B' "
                            End If
                        Else
                            strSql = "INSERT INTO PRODUCT_ATTACHMENT_TEMP(PA_PRODUCT_CODE,PA_HUB_FILENAME,PA_ATTACH_FILENAME,"
                            strSql &= "PA_FILESIZE, PA_TYPE, PA_SOURCE, PA_STATUS, PA_DATE) VALUES("
                            strSql &= "'" & strDocNo & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ", 'I', "
                            If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                                strSql &= "'H', 'T',GETDATE())"
                            Else
                                'Michelle (8/11/2010) - To cater for bim
                                'strSql &= "'V', 'T')"
                                If pFrontOfficeSite = "B" Then
                                    strSql &= "'B', 'T',GETDATE())"
                                Else
                                    strSql &= "'V', 'T',GETDATE())"
                                End If
                            End If
                        End If

                        'If strIndex = "" Then ' new added in item
                        '    strSql = "UPDATE PRODUCT_MSTR_TEMP SET PM_PRODUCT_IMAGE = '" & strDocNo & filename & "' "
                        '    strSql &= "WHERE PM_PRODUCT_INDEX = (SELECT MAX(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR_TEMP)"
                        'Else
                        '    strSql = "UPDATE PRODUCT_MSTR_TEMP SET PM_PRODUCT_IMAGE = '" & strDocNo & filename & "' "
                        '    strSql &= "WHERE PM_PRODUCT_INDEX = " & strIndex
                        'End If

                        'strSql = "INSERT INTO PRODUCT_ATTACHMENT_TEMP(PA_PRODUCT_CODE,PA_HUB_FILENAME,PA_ATTACH_FILENAME," _
                        '& "PA_FILESIZE) VALUES('" & strDocNo & "','" & strHubFile & "','" & filename & "'," & contentLength & ")"
                    Else
                        'strSql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & strDocNo & filename & "' "
                        If objDb.Exist("SELECT '*' FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & strDocNo & "' AND PA_TYPE = 'I'") > 0 Then
                            blnAdd = False
                            strSql = "UPDATE PRODUCT_ATTACHMENT SET "
                            strSql &= "PA_HUB_FILENAME = '" & strHubFile & "', "
                            strSql &= "PA_ATTACH_FILENAME = '" & Common.Parse(filename) & "', "
                            strSql &= "PA_FILESIZE = " & contentLength & " "
                            strSql &= "WHERE PA_PRODUCT_CODE = '" & strDocNo & "' "
                            strSql &= "AND PA_TYPE = 'I'; "
                            strSql &= "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & strIndex & "' "
                            strSql &= "WHERE PM_PRODUCT_CODE = '" & strDocNo & "' "
                        Else
                            ' new item
                            strSql = "INSERT INTO PRODUCT_ATTACHMENT(PA_PRODUCT_CODE,PA_HUB_FILENAME,PA_ATTACH_FILENAME,"
                            strSql &= "PA_FILESIZE, PA_TYPE) VALUES("
                            strSql &= "'" & strDocNo & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ", 'I'); "
                        End If

                        'strSql = "INSERT INTO PRODUCT_ATTACHMENT(PA_PRODUCT_CODE,PA_HUB_FILENAME,PA_ATTACH_FILENAME," _
                        '   & "PA_FILESIZE) VALUES('" & strDocNo & "','" & strHubFile & "','" & filename & "'," & contentLength & ")"
                    End If
                    '//return filename      
                Case EnumUploadType.ProductAttachment
                    strUploadPath = getSystemParam("ProductDocPath", "", strConnStr)
                    '     WriteLog("strUploadPath", strUploadPath)
                    If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                        strTempBasePath = getSystemParam("TempBaseUploadPath", System.Enum.GetName(GetType(EnumUploadFrom), pEnumUploadFrom), strConnStr)
                        'strFilePath = strTempBasePath & strUploadPath
                        strFilePath = strTempBasePath & strUploadPath & strCoyID & "\"
                        strMovePath = strBasePath & strUploadPath & strCoyID & "\"
                    Else
                        strFilePath = strBasePath & strUploadPath & strCoyID & "\"
                    End If
                    '   WriteLog("strFilePath", strFilePath)
                    'getFilePath(pEnumUploadFrom, EnumUploadType.ProductDocPath, strFilePath, strMovePath)
                    If blnTemp Then             ' insert into temp table
                        strSql = "INSERT INTO PRODUCT_ATTACHMENT_TEMP(PA_PRODUCT_CODE,PA_HUB_FILENAME,PA_ATTACH_FILENAME,"
                        strSql &= "PA_FILESIZE, PA_TYPE, PA_SOURCE, PA_STATUS, PA_DATE) VALUES("
                        strSql &= "'" & strDocNo & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ", 'D', "
                        If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                            strSql &= "'H','T',GETDATE())"
                        Else
                            'Michelle (8/11/2010) - To cater for buyer site
                            'strSql &= "'V', 'T')"
                            If pFrontOfficeSite = "B" Then
                                strSql &= "'B', 'T',GETDATE())"
                            Else
                                strSql &= "'V', 'T',GETDATE())"
                            End If
                        End If
                    Else
                        strSql = "INSERT INTO PRODUCT_ATTACHMENT(PA_PRODUCT_CODE,PA_HUB_FILENAME,PA_ATTACH_FILENAME,"
                        strSql &= "PA_FILESIZE, PA_TYPE) VALUES("
                        strSql &= "'" & strDocNo & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ", 'D')"
                    End If
                    '      WriteLog("strSql", strSql)

                    'Michelle (eBiz/303) - to allow AO to attach file
                Case EnumUploadType.UserAttachment
                    If contentType = "application/msword" Then

                    End If
                    strUploadPath = getSystemParam("DocAttachPath", "USER", strConnStr)
                    strFilePath = strBasePath & strUploadPath               '& strCoyID & "\"
                    '//update Company_Doc_Attachment
                    '//maybe need to check dup before insert
                    'strSql = "INSERT INTO User_Attachment(UA_COY_ID,UA_USER_ID,UA_USER_ROLE,UA_DOC_NO, UA_DOC_TYPE," _
                    '                    & "UA_HUB_FILENAME,UA_ATTACH_FILENAME,UA_FILESIZE) VALUES('" & strCoyID & "','" & _
                    '                    HttpContext.Current.Session("UserId") & "','" & strIndex & "', '" & _
                    '                    strDocNo & "','" & strDocType & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ")"


                    'Yik Foong
                    'Change the value store in TABLE USER_ATTACHMENT COLUMN UA_USER_ID in
                    ' [UserID]-[SequenceNumber] format
                    Dim seqNum As String = seq
                    Dim ua_user_id As String = userID
                    If seqNum <> "" Then
                        ua_user_id = userID & "-" & seqNum
                    End If

                    strSql = "INSERT INTO User_Attachment(UA_COY_ID,UA_USER_ID,UA_USER_ROLE,UA_DOC_NO, UA_DOC_TYPE," _
                      & "UA_HUB_FILENAME,UA_ATTACH_FILENAME,UA_FILESIZE) VALUES('" & strCoyID & "','" &
                      ua_user_id & "','" & strIndex & "', '" &
                      strDocNo & "','" & strDocType & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ")"

                Case EnumUploadType.DOAttachment
                    strUploadPath = getSystemParam("DocAttachPath", strDocType, strConnStr)
                    'If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                    '    strTempBasePath = getSystemParam("TempBaseUploadPath", System.Enum.GetName(GetType(EnumUploadFrom), pEnumUploadFrom), strConnStr)
                    '    strFilePath = strTempBasePath & strUploadPath & strCoyID & "\"
                    '    strMovePath = strBasePath & strUploadPath & strCoyID & "\"
                    'Else
                    strFilePath = strBasePath & strUploadPath & strCoyID & "\"
                    'End If
                    'If blnTemp Then             ' insert into temp table
                    strSql = "INSERT INTO COMPANY_DO_DOC_ATTACHMENT_TEMP "
                    strSql &= "(CDDA_COY_ID,CDDA_DOC_NO,CDDA_HUB_FILENAME,CDDA_ATTACH_FILENAME,CDDA_FILESIZE,CDDA_TYPE,CDDA_STATUS,CDDA_DATE"

                    If AttchType = "D" Then
                        strSql = strSql & ",CDDA_ITEM_CODE,CDDA_LINE_NO,CDDA_PO_LINE"
                    End If

                    strSql &= ")VALUES('" & strCoyID & "', '" & strDocNo & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ", '" & AttchType & "','N', NOW()"

                    If AttchType = "D" Then
                        strSql = strSql & ",'" & Common.Parse(ItemCode) & "','" & LineNo & "','" & POLine & "'"
                    End If

                    strSql = strSql & ")"

                    'If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                    '    strSql &= "'H','T',GETDATE())"
                    'Else
                    '    'Michelle (8/11/2010) - To cater for buyer site
                    '    'strSql &= "'V', 'T')"
                    '    If pFrontOfficeSite = "B" Then
                    '        strSql &= "'B', 'T',GETDATE())"
                    '    Else
                    '        strSql &= "'V', 'T',GETDATE())"
                    '    End If
                    'End If
                    'Else
                    'strSql = "INSERT INTO PRODUCT_ATTACHMENT(PA_PRODUCT_CODE,PA_HUB_FILENAME,PA_ATTACH_FILENAME,"
                    'strSql &= "PA_FILESIZE, PA_TYPE) VALUES("
                    'strSql &= "'" & strDocNo & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ", 'D')"
                    'End If

                Case EnumUploadType.CompDocAttachment
                    strUploadPath = getSystemParam("DocAttachPath", strDocType, strConnStr)
                    strFilePath = strBasePath & strUploadPath & strCoyID & "\"

                    strSql = "INSERT INTO COMPANY_DOC_ATTACHMENT_TEMP(CDA_COY_ID,CDA_DOC_TYPE," _
                    & "CDA_HUB_FILENAME,CDA_ATTACH_FILENAME,CDA_FILESIZE,CDA_TYPE,CDA_STATUS) VALUES('" & strCoyID & "','" &
                    strDocType & "','" & strHubFile & "','" & Common.Parse(filename) & "'," & contentLength & ",'" & AttchType & "','N')"

            End Select
            checkDirectory(strFilePath)
            'myFile.SaveAs(strFilePath & strHubFile)
            objDb.Execute(strSql)

            If pEnumUploadType = EnumUploadType.ProductImage Or pEnumUploadType = EnumUploadType.DocAttachment Or pEnumUploadType = EnumUploadType.DOAttachment Then
                If blnAdd Then
                    'Michelle (5/11/2010) - To cater for BIM
                    'strLatest = objDb.GetVal("SELECT ISNULL(MAX(PA_ATTACH_INDEX), 0) FROM PRODUCT_ATTACHMENT_TEMP")
                    If Not blnTemp And pEnumUploadType = EnumUploadType.ProductImage Then
                        strLatest = objDb.GetValDecimal("SELECT ISNULL(MAX(PA_ATTACH_INDEX), 0) FROM PRODUCT_ATTACHMENT")
                    ElseIf pEnumUploadType = EnumUploadType.DocAttachment Then
                        'Michelle (15/11/2010) - using GetVal will sometime have 'Multiple controls with the same ID' error
                        'strLatest = objDb.GetVal("SELECT ISNULL(MAX(PA_ATTACH_INDEX), 0) FROM PRODUCT_ATTACHMENT_TEMP")
                        strLatest = objDb.Get1Column("COMPANY_DOC_ATTACHMENT", "CDA_ATTACH_INDEX", " ORDER BY CDA_ATTACH_INDEX DESC ")
                    ElseIf pEnumUploadType = EnumUploadType.DOAttachment Then
                        strLatest = objDb.Get1Column("COMPANY_DO_DOC_ATTACHMENT_TEMP", "CDDA_ATTACH_INDEX", " ORDER BY CDDA_ATTACH_INDEX DESC ")
                    End If
                    'If pEnumUploadFrom = EnumUploadFrom.BackOff Then
                    '    strLatest = objDb.GetVal("SELECT ISNULL(MAX(PA_ATTACH_INDEX), 0) FROM PRODUCT_ATTACHMENT")
                    '    strSql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & strLatest & "' "
                    '    strSql &= "WHERE PM_PRODUCT_CODE = '" & strDocNo & "' "
                    '    objDb.Execute(strSql)
                    'Else
                    '    strLatest = objDb.GetVal("SELECT ISNULL(MAX(PA_ATTACH_INDEX), 0) FROM PRODUCT_ATTACHMENT_TEMP")
                    'End If
                Else
                    strLatest = strIndex
                End If
                FileUpload = strLatest
            End If

            '//cannot function, may need to use stored procedure to move image
            '---------remarked by ai chu (no cross server)-----------------
            'If pEnumUploadFrom = EnumUploadFrom.BackOff Then
            '    'If EnumUploadType.ProductImage Then
            '    '    If File.Exists(strFilePath & filename) Then
            '    '        File.Move(strFilePath & filename, strMovePath & filename)
            '    '    End If
            '    'End If
            '    If pEnumUploadType = EnumUploadType.ProductImage Or pEnumUploadType = EnumUploadType.ProductAttachment Then
            '        If File.Exists(strFilePath & strHubFile) Then
            '            'Dim strAry() As String
            '            'strAry = Directory.GetFiles("\\Scsdsb\WheelFileMgnt\ProductDoc\demo\")
            '            checkDirectory(strMovePath)
            '            File.Copy(strFilePath & strHubFile, strMovePath & strHubFile)
            '        End If
            '    End If
            'End If
            '------------------------------------------'
            If Not FileUpload Is Nothing Then
                FileUpload = strLatest
            Else
                FileUpload = filename
            End If

        End Function

        '//download term and condition file
        '//download PR,PO,RFQ attached
        Function FileDownload(ByVal companyID As String, ByVal strFileName As String, ByVal pEnumDownloadType As EnumDownLoadType, ByVal strDocType As String, ByVal pEnumUploadFrom As EnumUploadFrom, Optional ByVal strCoy As String = "", Optional ByVal strConnStr As String = Nothing) As String
            Dim strSql, strMsg, strBasePath, strDownloadPath, strFilePath, strCoyID As String
            Dim strTempBasePath, strMovePath As String

            If strCoy = "" Then
                If pEnumUploadFrom = EnumUploadFrom.BackOff Then
                    strCoyID = companyID
                Else
                    strCoyID = companyID
                End If
            Else
                strCoyID = strCoy
            End If

            If ConfigurationManager.AppSettings("CrossServer") Then
                strBasePath = getBasePath(pEnumUploadFrom, strConnStr)
            Else
                strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
            End If

            Select Case pEnumDownloadType
                Case EnumDownLoadType.DocAttachment
                    strDownloadPath = getSystemParam("DocAttachPath", strDocType, strConnStr)
                    'Jules 2019.02.28 - Commented out IF portion because system unable to retrieve image file using this path.
                    'If Common.isImageFile(strFileName) Then
                    '    strFilePath = getSystemParam("BaseDisplayPath", "", strConnStr) & strDownloadPath
                    'Else
                    strFilePath = strBasePath & strDownloadPath                      '& strCoyID & "\"
                    'End If

                    'Zulham 23072018 - PAMB
                    If strDocType = "IPP" And companyID.ToString.ToUpper = "PAMB" Then
                        strFilePath = strBasePath & strDownloadPath
                    End If
                    'End

                    'strFilePath = strBasePath & strDownloadPath '& strCoyID & "\"
                Case EnumDownLoadType.ProductImage
                    '//for display purpose, should be at virtual directory
                    strDownloadPath = getSystemParam("ProductImagePath", "", strConnStr)
                    'strFilePath = strBasePath & strDownloadPath & strCoyID & "\"
                    strFilePath = getSystemParam("BaseDisplayPath", "", strConnStr) & strDownloadPath & strCoyID & "\"
                    ' ai chu remark
                    ' strFilePath = getSystemParam("BaseDisplayPath", "") & strDownloadPath & strCoyID & "\"
                    'strBasePath = getBasePath(pEnumUploadFrom)
                    'strDisplayPath = getSystemParam("BaseDisplayPath", "")
                    ''strFilePath = strBasePath & strDownloadPath & strCoyId & "\" & strFile1
                    'strFilePath = strDisplayPath & strDownloadPath & strCoyId & "\" & strFile1
                Case EnumDownLoadType.ProductAttach
                    strDownloadPath = getSystemParam("ProductDocPath", "", strConnStr)
                    If Common.isImageFile(strFileName) Then
                        strFilePath = getSystemParam("BaseDisplayPath", "", strConnStr) & strDownloadPath & strCoyID & "\"
                    Else
                        strFilePath = strBasePath & strDownloadPath & strCoyID & "\"
                    End If

                Case EnumDownLoadType.CoyLogo
                    '//for display purpose, should be at virtual directory
                    '//cannot upload by hub
                    strDownloadPath = getSystemParam("CompanyLogo", "", strConnStr)
                Case EnumDownLoadType.TermAndCond
                    strDownloadPath = getSystemParam("TermAndCond", "", strConnStr)
                    If Common.isImageFile(strFileName) Then
                        strFilePath = getSystemParam("BaseDisplayPath", "", strConnStr) & strDownloadPath
                    Else
                        strFilePath = strBasePath & strDownloadPath                      '& "\"
                    End If

                    'Case EnumDownLoadType.PO_TermAndCond
                    '    strDownloadPath = getSystemParam("DocAttachPath", "PO")
                    '    strFilePath = strBasePath & strDownloadPath '& strCoyID & "\"

                Case EnumDownLoadType.UserAttachment                'Michelle (eBiz/303) - To cater for AO attachment
                    strDownloadPath = getSystemParam("DocAttachPath", "USER", strConnStr)
                    If Common.isImageFile(strFileName) Then
                        strFilePath = getSystemParam("BaseDisplayPath", "", strConnStr) & strDownloadPath
                    Else
                        strFilePath = strBasePath & strDownloadPath                      '& strCoyID & "\"
                    End If
                Case EnumDownLoadType.DOAttachment
                    strDownloadPath = getSystemParam("DocAttachPath", strDocType, strConnStr)
                    'Jules 2019.01.22 - Commented out IF portion because system unable to retrieve image file using this path.
                    'If Common.isImageFile(strFileName) Then
                    '    strFilePath = getSystemParam("BaseDisplayPath", "", strConnStr) & strDownloadPath & strCoyID & "\"
                    'Else
                    strFilePath = strBasePath & strDownloadPath & strCoyID & "\"
                    'End If
                Case EnumDownLoadType.CompDocAttachment
                    strDownloadPath = getSystemParam("DocAttachPath", strDocType, strConnStr)
                    If Common.isImageFile(strFileName) Then
                        strFilePath = getSystemParam("BaseDisplayPath", "", strConnStr) & strDownloadPath & strCoyID & "\"
                    Else
                        strFilePath = strBasePath & strDownloadPath & strCoyID & "\"
                    End If
            End Select
            Return strFilePath & strFileName
        End Function

        Function copyPRAttachToPO(ByVal strCoyID As String, ByVal strPRNo As String, ByVal strPONo As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strSql, strMsg, strBasePath, strPRPath, strPOPath As String
            Dim strSourceFile, strDestFile, strAttchIndex As String
            Dim objDb As New EAD.DBCom

            If Not strConnStr Is Nothing Then
                objDb = New EAD.DBCom(strConnStr)
            End If

            strAttchIndex = ""
            strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
            strPRPath = getSystemParam("DocAttachPath", "PR", strConnStr)
            strPOPath = getSystemParam("DocAttachPath", "PO", strConnStr)

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_DOC_TYPE='PR' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_COY_ID='" & strCoyID & "'"

            Dim tDS As DataSet = objDb.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")) Then
                    'strSourceFile = strBasePath & strPRPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                    strSourceFile = strBasePath & strPRPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                    If File.Exists(strSourceFile) Then
                        'checkDirectory(strBasePath & strPRPath & strCoyID & "\")
                        checkDirectory(strBasePath & strPRPath)
                        checkDirectory(strBasePath & strPOPath)
                        'strDestFile = strBasePath & strPOPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                        strDestFile = strBasePath & strPOPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                        If File.Exists(strDestFile) Then
                            File.Delete(strDestFile)
                        End If
                        File.Copy(strSourceFile, strDestFile)
                        If strAttchIndex = "" Then
                            strAttchIndex = tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        Else
                            strAttchIndex = strAttchIndex & "," & tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        End If
                    End If
                End If
            Next

            objDb = Nothing
            Return strAttchIndex
        End Function

        Function copyPRAttachToPOMulti(ByVal strPRNo As String, ByVal strPONo As String, Optional ByVal strConnStr As String = Nothing, Optional ByVal strExtraCheck As String = "") As String
            Dim strSql, strMsg, strBasePath, strPRPath, strPOPath, strCoyID, strRFQPath As String
            Dim strSourceFile, strDestFile, strAttchIndex As String
            Dim objDb As New EAD.DBCom

            If Not strConnStr Is Nothing Then
                objDb = New EAD.DBCom(strConnStr)
            End If

            strAttchIndex = ""
            strCoyID = HttpContext.Current.Session("CompanyId")
            strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
            strPRPath = getSystemParam("DocAttachPath", "PR", strConnStr)
            strPOPath = getSystemParam("DocAttachPath", "PO", strConnStr)
            strRFQPath = getSystemParam("DocAttachPath", "RFQ", strConnStr)

            If strExtraCheck = "RFQ" Then
                strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_DOC_TYPE='RFQ' AND CDA_DOC_NO = '" & strPRNo & "' AND CDA_COY_ID='" & strCoyID & "' AND CDA_TYPE = 'E' "
            ElseIf strExtraCheck = "PRInt" Then
                strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_DOC_TYPE='PR' AND CDA_DOC_NO IN (" & strPRNo & ") AND CDA_COY_ID='" & strCoyID & "' AND CDA_TYPE = 'I' "
            Else
                strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_DOC_TYPE='PR' AND CDA_DOC_NO IN (" & strPRNo & ") AND CDA_COY_ID='" & strCoyID & "'"
            End If

            Dim tDS As DataSet = objDb.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")) Then
                    'strSourceFile = strBasePath & strPRPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                    If strExtraCheck = "RFQ" Then
                        strSourceFile = strBasePath & strRFQPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                    Else
                        strSourceFile = strBasePath & strPRPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                    End If

                    If File.Exists(strSourceFile) Then
                        'checkDirectory(strBasePath & strPRPath & strCoyID & "\")
                        checkDirectory(strBasePath & strPRPath)
                        checkDirectory(strBasePath & strPOPath)
                        checkDirectory(strBasePath & strRFQPath)
                        'strDestFile = strBasePath & strPOPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                        strDestFile = strBasePath & strPOPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                        If File.Exists(strDestFile) Then
                            File.Delete(strDestFile)
                        End If
                        File.Copy(strSourceFile, strDestFile)
                        If strAttchIndex = "" Then
                            strAttchIndex = tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        Else
                            strAttchIndex = strAttchIndex & "," & tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        End If
                    End If
                End If
            Next

            objDb = Nothing
            Return strAttchIndex
        End Function

        Function copyPRAttachToRFQ(ByVal strPRNo As String, ByVal strRFQNo As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strSql, strMsg, strBasePath, strPRPath, strRFQPath, strCoyID As String
            Dim strSourceFile, strDestFile, strAttchIndex As String
            Dim objDb As New EAD.DBCom

            If Not strConnStr Is Nothing Then
                objDb = New EAD.DBCom(strConnStr)
            End If

            strAttchIndex = ""
            strCoyID = HttpContext.Current.Session("CompanyId")
            strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
            strPRPath = getSystemParam("DocAttachPath", "PR", strConnStr)
            strRFQPath = getSystemParam("DocAttachPath", "RFQ", strConnStr)

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_DOC_TYPE='PR' AND CDA_DOC_NO='" & strPRNo & "' AND CDA_COY_ID='" & strCoyID & "'"

            Dim tDS As DataSet = objDb.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")) Then
                    'strSourceFile = strBasePath & strPRPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                    strSourceFile = strBasePath & strPRPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                    If File.Exists(strSourceFile) Then
                        'checkDirectory(strBasePath & strPRPath & strCoyID & "\")
                        checkDirectory(strBasePath & strPRPath)
                        checkDirectory(strBasePath & strRFQPath)
                        'strDestFile = strBasePath & strPOPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                        strDestFile = strBasePath & strRFQPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                        If File.Exists(strDestFile) Then
                            File.Delete(strDestFile)
                        End If
                        File.Copy(strSourceFile, strDestFile)
                        If strAttchIndex = "" Then
                            strAttchIndex = tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        Else
                            strAttchIndex = strAttchIndex & "," & tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        End If
                    End If
                End If
            Next

            objDb = Nothing
            Return strAttchIndex
        End Function

        Function copyPRAttachToRFQMulti(ByVal strPRNo As String, ByVal strRFQNo As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strSql, strMsg, strBasePath, strPRPath, strRFQPath, strCoyID As String
            Dim strSourceFile, strDestFile, strAttchIndex As String
            Dim objDb As New EAD.DBCom

            If Not strConnStr Is Nothing Then
                objDb = New EAD.DBCom(strConnStr)
            End If

            strAttchIndex = ""
            strCoyID = HttpContext.Current.Session("CompanyId")
            strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
            strPRPath = getSystemParam("DocAttachPath", "PR", strConnStr)
            strRFQPath = getSystemParam("DocAttachPath", "RFQ", strConnStr)

            strSql = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_DOC_TYPE='PR' AND CDA_DOC_NO IN (" & strPRNo & ") AND CDA_COY_ID='" & strCoyID & "'"

            Dim tDS As DataSet = objDb.FillDs(strSql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If Not IsDBNull(tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")) Then
                    'strSourceFile = strBasePath & strPRPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                    strSourceFile = strBasePath & strPRPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                    If File.Exists(strSourceFile) Then
                        'checkDirectory(strBasePath & strPRPath & strCoyID & "\")
                        checkDirectory(strBasePath & strPRPath)
                        checkDirectory(strBasePath & strRFQPath)
                        'strDestFile = strBasePath & strPOPath & strCoyID & "\" & drAttach("CDA_HUB_FILENAME")
                        strDestFile = strBasePath & strRFQPath & tDS.Tables(0).Rows(j).Item("CDA_HUB_FILENAME")
                        If File.Exists(strDestFile) Then
                            File.Delete(strDestFile)
                        End If
                        File.Copy(strSourceFile, strDestFile)
                        If strAttchIndex = "" Then
                            strAttchIndex = tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        Else
                            strAttchIndex = strAttchIndex & "," & tDS.Tables(0).Rows(j).Item("CDA_ATTACH_INDEX")
                        End If
                    End If
                End If
            Next

            objDb = Nothing
            Return strAttchIndex
        End Function

        '//If uploaded by hubadmin,save as at TempBaseUploadPath + ProductImagePath then move to BaseUploadPath + ProductImagePath
        '//because cannot upload to Map Drive
        '//If uploaded by ven admin,save as at BaseUploadPath + ProductImagePath
        'Function uploadProductImage(ByVal pBrowseFile As HtmlInputFile, ByVal pEnumUploadFrom As EnumUploadFrom)

        'End Function
        '//multiple attachment.
        'Function uploadProductAttach(ByVal pBrowseFile() As HttpPostedFile, ByVal pEnumUploadFrom As EnumUploadFrom, ByVal strPodCode As String, ByRef strAryQuery() As String)
        '    Dim intCnt, intLoop As Integer
        '    Dim tempBrowseFile As HttpPostedFile
        '    Dim filename As String
        '    Dim contentType As String
        '    Dim contentLength As String
        '    intCnt = pBrowseFile.GetUpperBound(0)

        '    Dim strSql, strMsg, strBasePath, strUploadPath, strFilePath, strCoyID As String
        '    Dim strTempBasePath, strMovePath As String
        '    Dim strHubFile As String

        '    '//get uploadPath
        '    strBasePath = getBasePath(pEnumUploadFrom)
        '    strUploadPath = getSystemParam("ProductDocPath", "")
        '    If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
        '        strTempBasePath = getSystemParam("TempBaseUploadPath", System.Enum.GetName(GetType(EnumUploadFrom), pEnumUploadFrom))
        '        strFilePath = strTempBasePath & strUploadPath & strCoyID & "\"
        '        strMovePath = strBasePath & strUploadPath & strCoyID & "\"
        '        checkDirectory(strMovePath)
        '    Else
        '        strFilePath = strBasePath & strUploadPath & strCoyID & "\"
        '    End If
        '    checkDirectory(strFilePath)
        '    '//get uploadPath
        '    For intLoop = 0 To intCnt
        '        '//get uploaded file detail
        '        tempBrowseFile = pBrowseFile(intLoop)
        '        filename = System.IO.Path.GetFileName(tempBrowseFile.FileName)
        '        contentType = tempBrowseFile.ContentType
        '        contentLength = tempBrowseFile.ContentLength
        '        contentLength = contentLength / 1024 'convert to KB
        '        '//get uploaded file detail

        '        '//get unique hub file name
        '        strHubFile = getLastDocAttachFileName(filename)
        '        '//update Product_Attachment
        '        tempBrowseFile.SaveAs(strFilePath & filename)
        '        strSql = "INSERT INTO PRODUCT_ATTACHMENT(PA_PRODUCT_CODE,PA_HUB_FILENAME," _
        '        & "PA_ATTACH_FILENAME,PA_FILESIZE) VALUES('" & strPodCode & "','" & _
        '        strHubFile & "','" & filename & "'," & contentLength & ")"
        '        Common.Insert2Ary(strAryQuery, strSql)
        '        '//need to call stored procedure or use other method -- pending
        '        If pEnumUploadFrom = EnumUploadFrom.BackOff Then
        '            If File.Exists(strFilePath & filename) Then
        '                File.Move(strFilePath & filename, strMovePath & filename)
        '            End If
        '        End If
        '    Next
        'End Function
        'return parameter - FileName
        '//old system - rename and copy the T&C for every PO
        '//New system - get HubFileName from Company_Mstr, store in PO_MSTR
        Function copyTermCondToPO(ByVal companyid As String, ByVal strPONo As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strBasePath, strUploadPath, strTermPath, strSql, strTermFile As String
            Dim strTempFile, strTempFile2, strHubFile As String
            Dim objDb As EAD.DBCom

            strSql = "SELECT ISNULL(CM_HUB_TERMSANDCONDFILE,'') FROM COMPANY_MSTR WHERE CM_COY_ID='" & companyid & "'"

            If strConnStr Is Nothing Then
                objDb = New EAD.DBCom
            Else
                objDb = New EAD.DBCom(strConnStr)
            End If

            strTermFile = objDb.GetVal(strSql)
            If strTermFile <> "" Then
                strTermPath = getSystemParam("TermAndCond", "", strConnStr)
                strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
                strTempFile = strBasePath & strTermPath & "\" & strTermFile
                If File.Exists(strTempFile) Then
                    'strUploadPath = getSystemParam("DocAttachPath", "PO")
                    'strTempFile2 = strBasePath & strUploadPath & strCoyID & "\"
                    'checkDirectory(strTempFile2)
                    'strHubFile = getLastDocAttachFileName(strTermFile)
                    'strTempFile2 = strTempFile2 & strHubFile
                    'File.Copy(strTempFile, strTempFile2)
                    ''//update PO_MSTR
                    'Return strTermFile
                    Return strTermFile
                Else
                    '//no file
                    Return String.Empty
                End If
            Else
                '//no file
                Return String.Empty
            End If
        End Function

        Function getCoyLogo(ByVal pEnumUploadFrom As EnumUploadFrom, ByVal strCoyID As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strSql, strBasePath, strDownloadPath, strFilePath, strLogo, strDisplayPath As String

            If Not strConnStr Is Nothing Then
                objDb = New EAD.DBCom(strConnStr)
            End If

            If strCoyID = "" Then
                strCoyID = HttpContext.Current.Session("CompanyId")
            End If

            If ConfigurationManager.AppSettings("CrossServer") Then
                strBasePath = getBasePath(pEnumUploadFrom, strConnStr)
            Else
                strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
            End If

            strDownloadPath = getSystemParam("CoyLogo", "", strConnStr)
            strDisplayPath = getSystemParam("BaseDisplayPath", "", strConnStr)
            strFilePath = strBasePath & strDownloadPath          '& "\"                    

            strSql = "SELECT ISNULL(CM_COY_LOGO,'') FROM COMPANY_MSTR WHERE CM_COY_ID='" & strCoyID & "'"
            'strSql = "SELECT ISNULL(CM_COY_LOGO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "'" & _
            '    " UNION " & _
            '    " SELECT ISNULL(CM_COY_LOGO, '') FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID = '" & strCoyID & "'"

            strLogo = objDb.GetVal(strSql)

            'If pEnumUploadFrom = pEnumUploadFrom.FrontOff Then
            If File.Exists(strFilePath & strLogo) Then
                Return strDisplayPath & strDownloadPath & strLogo
            Else
                Return String.Empty
            End If
        End Function

        Function getReportCoyLogo(ByVal pEnumUploadFrom As EnumUploadFrom, ByVal strCoyID As String, ByVal strReportPath As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strSql, strBasePath, strDownloadPath, strFilePath, strLogo, strDisplayPath As String

            If Not strConnStr Is Nothing Then
                objDb = New EAD.DBCom(strConnStr)
            End If

            If strCoyID = "" Then
                strCoyID = HttpContext.Current.Session("CompanyId")
            End If

            If ConfigurationManager.AppSettings("CrossServer") Then
                strBasePath = getBasePath(pEnumUploadFrom, strConnStr)
            Else
                strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
            End If

            strDownloadPath = getSystemParam("CoyLogo", "", strConnStr)
            strDisplayPath = getSystemParam("BaseDisplayPath", "", strConnStr)
            strFilePath = strBasePath & strDownloadPath          '& "\"                    

            strSql = "SELECT ISNULL(CM_COY_LOGO,'') FROM COMPANY_MSTR WHERE CM_COY_ID='" & strCoyID & "'"
            'strSql = "SELECT ISNULL(CM_COY_LOGO, '') FROM COMPANY_MSTR WHERE CM_COY_ID = '" & strCoyID & "'" & _
            '    " UNION " & _
            '    " SELECT ISNULL(CM_COY_LOGO, '') FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID = '" & strCoyID & "'"

            strLogo = objDb.GetVal(strSql)

            'If pEnumUploadFrom = pEnumUploadFrom.FrontOff Th en
            If File.Exists(strFilePath & strLogo) Then
                FileCopy(strFilePath & strLogo, strReportPath & strLogo)
                Return strLogo
            Else
                Return String.Empty
            End If
        End Function

        Function uploadCoyLogo(ByVal pBrowseFile As HtmlInputFile, ByVal pEnumUploadFrom As EnumUploadFrom, ByVal strCoyID As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strBasePath, strUploadPath, strFilePath, strTempBasePath, strMovePath As String
            Dim strSaveAsFile As String

            Dim myFile As HttpPostedFile
            myFile = pBrowseFile.PostedFile
            Dim filename As String = System.IO.Path.GetFileName(myFile.FileName)
            'strSaveAsFile = filename
            Dim iPos As Integer = InStr(filename, ".")
            If iPos > 0 Then strSaveAsFile = filename.Substring(iPos - 1)
            strSaveAsFile = getLastDocAttachFileName(strCoyID & strSaveAsFile, strConnStr)


            getFilePath(pEnumUploadFrom, EnumUploadType.CoyLogo, strFilePath, strMovePath, , , strConnStr)

            checkDirectory(strFilePath)
            myFile.SaveAs(strFilePath & strSaveAsFile)
            If ConfigurationManager.AppSettings("CrossServer") Then
                If pEnumUploadFrom = EnumUploadFrom.BackOff Then
                    'Move Image 
                    checkDirectory(strMovePath)
                    File.Move(strFilePath & strSaveAsFile, strMovePath & strSaveAsFile)
                End If
            End If
            Return strSaveAsFile
        End Function

        Sub uploadCoyTC(ByVal pBrowseFile As HtmlInputFile, ByVal pEnumUploadFrom As EnumUploadFrom, ByVal strCoyID As String, ByRef strActualTC As String, ByRef strHubTC As String, Optional ByVal strConnStr As String = Nothing)
            Dim strBasePath, strUploadPath, strFilePath, strTempBasePath, strMovePath As String
            Dim strSaveAsFile, strHubFile As String

            Dim myFile As HttpPostedFile
            myFile = pBrowseFile.PostedFile
            Dim filename As String = System.IO.Path.GetFileName(myFile.FileName)
            strHubFile = getLastDocAttachFileName(filename, strConnStr)
            'strSaveAsFile = strCoyID & "_" & filename
            'strSaveAsFile = filename
            getFilePath(pEnumUploadFrom, EnumUploadType.TermAndCond, strFilePath, strMovePath, , , strConnStr)

            checkDirectory(strFilePath)
            myFile.SaveAs(strFilePath & strHubFile)
            If ConfigurationManager.AppSettings("CrossServer") Then
                If pEnumUploadFrom = EnumUploadFrom.BackOff Then
                    'Move Image 
                    checkDirectory(strMovePath)
                    File.Move(strFilePath & strHubFile, strMovePath & strHubFile)
                End If
            End If

            strActualTC = filename
            strHubTC = strHubFile
        End Sub

        Function getSystemParam(ByVal strParamName As String, ByVal strParamType As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim objDb As EAD.DBCom
            Dim strSql, strTemp As String

            If strParamName <> "" Then
                strSql = "SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_NAME='" & strParamName & "'"
            Else
                Return Nothing
            End If
            If strParamType <> "" Then
                strSql = strSql & " AND SP_PARAM_TYPE='" & strParamType & "'"
            End If

            If strConnStr Is Nothing Then
                objDb = New EAD.DBCom
            Else
                objDb = New EAD.DBCom(strConnStr)
            End If

            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                strTemp = Common.parseNull(tDS.Tables(0).Rows(0).Item("SP_PARAM_VALUE"))
            Else
                Return "NULL"
            End If
            objDb = Nothing
            Return strTemp
        End Function
        Public Function getAttachment(ByVal strDocNo As String, ByVal strAttachIndex As String, ByVal bcomid As String, ByVal strType As String, ByVal _serverExtAddress As String) As String
            Dim strsql As String
            'Dim ds As New DataSet
            strsql = String.Format("SELECT CDA_ATTACH_INDEX, CDA_COY_ID, CDA_DOC_NO, CDA_DOC_TYPE, CDA_HUB_FILENAME, CDA_ATTACH_FILENAME, CDA_FILESIZE, CDA_TYPE, CONCAT('{0}', CDA_HUB_FILENAME) AS PathRef FROM COMPANY_DOC_ATTACHMENT ", _serverExtAddress)
            strsql &= "WHERE CDA_COY_ID = '" & bcomid & "' "
            strsql &= "AND CDA_DOC_NO IN (" & strDocNo & ") AND CDA_DOC_TYPE = '" & Common.Parse(strType) & "' "
            strsql &= If(Not String.IsNullOrEmpty(strAttachIndex), "AND CDA_ATTACH_INDEX IN (" & strAttachIndex & ")", String.Empty)
            'ds = objDb.FillDs(strsql)
            getAttachment = strsql
        End Function
        Public Function delAttachment(ByVal strDocNo As String, ByVal strAttachIndex As String, ByVal bcomid As String, ByVal strType As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
            strsql &= "WHERE CDA_COY_ID = '" & bcomid & "' "
            strsql &= "AND CDA_DOC_NO = '" & Common.Parse(strDocNo) & "' AND CDA_DOC_TYPE = '" & Common.Parse(strType) & "' "
            strsql &= If(Not String.IsNullOrEmpty(strAttachIndex), "AND CDA_ATTACH_INDEX = '" & strAttachIndex & "'", String.Empty)
            ds = objDb.FillDs(strsql)
            delAttachment = ds
        End Function

        Function getBasePath(ByVal pEnumUploadFrom As EnumUploadFrom, Optional ByVal strConnStr As String = Nothing) As String
            Dim strBasePath As String
            strBasePath = getSystemParam("BaseUploadPath", System.Enum.GetName(GetType(EnumUploadFrom), pEnumUploadFrom), strConnStr)
            Return strBasePath
        End Function

        Function checkDirectory(ByVal strFilePath As String) As Boolean
            If Not Directory.Exists(strFilePath) Then
                Directory.CreateDirectory(strFilePath)
            End If
        End Function
        '//get a file name unique in the file
        '//cannot use file name as per attached by user because they may attach files with same file name
        Function getLastDocAttachFileName(ByVal filename As String, Optional ByVal strConnStr As String = Nothing) As String
            Dim strLastUsedNo, strHubFile, strTempFile, strExt As String
            Dim intDotPos, i As Integer
            Dim strSql, strCheckDot As String
            Dim objDB As EAD.DBCom

            If strConnStr Is Nothing Then
                objDB = New EAD.DBCom
            Else
                objDB = New EAD.DBCom(strConnStr)
            End If

            strLastUsedNo = getSystemParam("AttachLastUsedNo", "", strConnStr)
            If strLastUsedNo = "NULL" Then
                strLastUsedNo = "0"
                strSql = "INSERT INTO SYSTEM_PARAM (SP_PARAM_NAME, SP_PARAM_VALUE, SP_PARAM_TYPE, SP_REMARK) VALUES ("
                strSql &= "'AttachLastUsedNo', 1, 'Item', 'Attach Item')"
            Else
                If strLastUsedNo = "" Then
                    strLastUsedNo = "0"
                End If
                strSql = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE='" & (Convert.ToInt64(strLastUsedNo) + 1).ToString & "' WHERE SP_PARAM_NAME='AttachLastUsedNo'"
            End If

            intDotPos = 0
            For i = 0 To filename.Length - 1
                strCheckDot = Mid(filename, filename.Length - i, 1)
                'strCheckDot = Mid(filename, filename.Length - i, filename.Length - i)
                If strCheckDot = "." Then
                    intDotPos = filename.Length - i
                    Exit For
                End If
            Next

            'intDotPos = InStr(filename, ".")
            If intDotPos > 0 Then
                strLastUsedNo = (Convert.ToInt64(strLastUsedNo) + 1).ToString
                strTempFile = Mid(filename, 1, intDotPos - 1)
                strExt = Mid(filename, intDotPos)
                strHubFile = strLastUsedNo & strExt
                'strSql = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE='" & strLastUsedNo & "' WHERE SP_PARAM_NAME='AttachLastUsedNo'"
                If objDB.Execute(strSql) Then
                    Return strHubFile
                Else
                    Return String.Empty
                End If
            Else
                '//no a valid file
                '//impossible??
                Return String.Empty
            End If
        End Function

        Sub getFilePath(ByVal pEnumUploadFrom As EnumUploadFrom, ByVal pEnumUploadType As EnumUploadType, ByRef strFilePath As String, ByRef strMovePath As String, Optional ByVal blnAppendCoy As Boolean = False, Optional ByVal strParamType As String = "", Optional ByVal strConnStr As String = Nothing)
            '//blnAppendCoy - Whether append CoyID as Folder Name
            Dim strBasePath, strUploadPath, strTempBasePath, strCoyID As String

            strUploadPath = getSystemParam(System.Enum.GetName(GetType(EnumUploadType), pEnumUploadType), strParamType, strConnStr)
            strMovePath = ""

            If blnAppendCoy Then
                If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                    strCoyID = HttpContext.Current.Session("CompanyIdToken")
                Else
                    strCoyID = HttpContext.Current.Session("CompanyId")
                End If
            End If

            If ConfigurationManager.AppSettings("CrossServer") Then
                strBasePath = getBasePath(pEnumUploadFrom, strConnStr)
                If pEnumUploadFrom = pEnumUploadFrom.BackOff Then
                    strTempBasePath = getSystemParam("TempBaseUploadPath", System.Enum.GetName(GetType(EnumUploadFrom), pEnumUploadFrom), strConnStr)
                    strFilePath = strTempBasePath & strUploadPath
                    strMovePath = strBasePath & strUploadPath
                Else
                    strFilePath = strBasePath & strUploadPath
                End If
            Else
                strBasePath = getBasePath(EnumUploadFrom.FrontOff, strConnStr)
                strFilePath = strBasePath & strUploadPath
            End If

            If blnAppendCoy Then
                strFilePath = strFilePath & strCoyID & "\"
                If strMovePath <> "" Then strMovePath = strMovePath & strCoyID & "\"
            End If
        End Sub

        ' ai chu modified on 05/10/2005
        ' when buyer wants to download attachment for list price item from vendor company,
        ' need to pass in vendor company id
        Public Function getAttachPath(ByVal pStrFile As String, ByVal pStrFileRaw As String, ByVal pStrFileName As String, ByVal pEnumDownloadType As EnumDownLoadType, ByVal pStrDocType As String, ByVal pEnumUploadFrom As EnumUploadFrom, Optional ByVal strConnStr As String = Nothing, Optional ByVal strCoyId As String = "") As String
            Dim dDispatcher = New dispatcher
            If Common.isImageFile(pStrFile) Then
                Dim strFilePath As String
                strFilePath = FileDownload(pStrFileName, pEnumDownloadType, pStrDocType, pEnumUploadFrom, strCoyId, strConnStr)
                strFilePath = strFilePath.Replace("\", "\\")
                getAttachPath = "<A HREF='#' onclick='PreviewAttach(""" & strFilePath & """);'>" & pStrFileRaw & "</A>"
            Else
                If pStrDocType = "" Then                'target='_blank'
                    'getAttachPath = "<A  HREF=../FileDownload.aspx?file=" & pStrFileName & "&actualfile=" & pStrFile & "&type=" & pEnumDownloadType & "&coyId=" & strCoyId & ">" & pStrFileRaw & "</A>"
                    getAttachPath = "<a href=""" & dDispatcher.direct("Initial", "FileDownload.aspx", "file=" & pStrFileName & "&actualfile=" & pStrFile & "&type=" & pEnumDownloadType & "&coyId=" & strCoyId) & """>" & pStrFileRaw & "</a>"
                Else
                    'getAttachPath = "<A  HREF=../FileDownload.aspx?file=" & pStrFileName & "&actualfile=" & pStrFile & "&type=" & pEnumDownloadType & "&doctype=" & pStrDocType & "&coyId=" & strCoyId & ">" & pStrFileRaw & "</A>"
                    getAttachPath = "<a href=""" & dDispatcher.direct("Initial", "FileDownload.aspx", "file=" & pStrFileName & "&actualfile=" & pStrFile & "&type=" & pEnumDownloadType & "&doctype=" & pStrDocType & "&coyId=" & strCoyId) & """>" & pStrFileRaw & "</a>"
                End If
            End If
        End Function
        'Zulham 19072018 - PAMB
        Public Function getAttachPathE2P(ByVal pStrFile As String, ByVal pStrFileRaw As String, ByVal pStrFileName As String, ByVal pEnumDownloadType As EnumDownLoadType, ByVal pStrDocType As String, ByVal pEnumUploadFrom As EnumUploadFrom, Optional ByVal strConnStr As String = Nothing, Optional ByVal strCoyId As String = "") As String
            Dim dDispatcher = New dispatcher
            If pStrDocType = "" Then
                getAttachPathE2P = "<a href=""" & dDispatcher.direct("Initial", "FileDownload.aspx", "file=" & pStrFileName & "&actualfile=" & pStrFile & "&type=" & pEnumDownloadType & "&coyId=" & strCoyId) & """>" & pStrFileRaw & "</a>"
            Else
                getAttachPathE2P = "<a href=""" & dDispatcher.direct("Initial", "FileDownload.aspx", "file=" & pStrFileName & "&actualfile=" & pStrFile & "&type=" & pEnumDownloadType & "&doctype=" & pStrDocType & "&coyId=" & strCoyId) & """>" & pStrFileRaw & "</a>"
            End If
            'End If
        End Function

    End Class
End Namespace

