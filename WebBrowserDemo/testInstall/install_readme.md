#nsis��װ���޸����İ�װ�����ʾ 

�޸Ļ����������� 
Var My_FINISHPAGE_TEXT 
Var returnValue 
; Welcome page 
!insertmacro MUI_PAGE_WELCOME 
; Instfiles page 
!insertmacro MUI_PAGE_INSTFILES 
; Finish page 
!define MUI_FINISHPAGE_TEXT "$My_FINISHPAGE_TEXT" 
!insertmacro MUI_PAGE_FINISH 

; ��ʼ�� 
Function .onInit 
  StrCpy $My_FINISHPAGE_TEXT ${SAG_EN_PRODUCT_NAME}��װ�ɹ�... 
FunctionEnd 

##����һ�����ݵ��õ�exe��main�����ķ���ֵ�ж��Ƿ�ɹ� 
��Section���λ���޸ģ� 
  ExecWait "$PROGRAMFILES\${SAG_EN_PRODUCT_NAME}\Tempbin\${SAG_FILE_PREFIX}WebBrowserDemo.exe" $returnValue 
  ; ���ݷ��ؽ���жϳɹ���ʧ�ܣ����յ��ò�ͬ�ĺ��� 
  IntCmp $returnValue 0 success 
  DetailPrint "${PRODUCT_NAME} ${PRODUCT_VERSION} Installed Failure " 
  ;MessageBox MB_OKCANCEL "ʧ�� --$returnValue--" 
  call funFail 
  Goto end 
  success: 
  DetailPrint "${PRODUCT_NAME} ${PRODUCT_VERSION} Installed Success " 
  ;MessageBox MB_OK "hi test! --$returnValue--" 
  call funSuccess 
  end: 
������ 
; ��װ�ɹ� 
Function funSuccess 
FunctionEnd 

; ��װʧ�� 
Function funFail 
  StrCpy $My_FINISHPAGE_TEXT ${SAG_EN_PRODUCT_NAME}��װʧ��... 
FunctionEnd 

##������������ĳ���ļ��Ƿ�����ж��Ƿ�ɹ� 
��Section���λ���޸ģ� 
  ExecWait "$PROGRAMFILES\${SAG_EN_PRODUCT_NAME}\Tempbin\${SAG_FILE_PREFIX}WebBrowserDemo.exe" 
  call funFinish 
������ 
; ִ��exe֮����÷����ж��ļ��Ƿ���� 
Function funFinish 
; �����ļ��Ƿ�����ж��Ƿ�ɹ� 
  IfFileExists $PROGRAMFILES\${SAG_EN_PRODUCT_NAME}\aa.exe 0 fail 
    ;MessageBox MB_OK "hi testaaa! $My_FINISHPAGE_TEXT" 
    Goto end 
  fail: 
    StrCpy $My_FINISHPAGE_TEXT ${SAG_EN_PRODUCT_NAME}��װʧ��aaa... 
    ;MessageBox MB_OK "hi testbbb! $My_FINISHPAGE_TEXT" 
  end: 
FunctionEnd 