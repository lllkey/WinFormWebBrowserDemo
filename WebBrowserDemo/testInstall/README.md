# nsis安装包修改最后的安装结果显示 

### 修改基本变量定义 
<pre><code>
Var My_FINISHPAGE_TEXT 
Var returnValue 
; Welcome page 
!insertmacro MUI_PAGE_WELCOME 
; Instfiles page 
!insertmacro MUI_PAGE_INSTFILES 
; Finish page 
!define MUI_FINISHPAGE_TEXT "$My_FINISHPAGE_TEXT" 
!insertmacro MUI_PAGE_FINISH 

; 初始化 
Function .onInit 
  StrCpy $My_FINISHPAGE_TEXT ${SAG_EN_PRODUCT_NAME}安装成功... 
FunctionEnd 
</code></pre>

## 方法一：根据调用的exe的main函数的返回值判断是否成功 
### 在Section相关位置修改： 
<pre><code>
  ExecWait "$PROGRAMFILES\${SAG_EN_PRODUCT_NAME}\Tempbin\${SAG_FILE_PREFIX}WebBrowserDemo.exe" $returnValue 
  ; 根据返回结果判断成功、失败，最终调用不同的函数 
  IntCmp $returnValue 0 success 
  DetailPrint "${PRODUCT_NAME} ${PRODUCT_VERSION} Installed Failure " 
  ;MessageBox MB_OKCANCEL "失败 --$returnValue--" 
  call funFail 
  Goto end 
  success: 
  DetailPrint "${PRODUCT_NAME} ${PRODUCT_VERSION} Installed Success " 
  ;MessageBox MB_OK "hi test! --$returnValue--" 
  call funSuccess 
  end: 
</code></pre>
### 函数： 
<pre><code>
; 安装成功 
Function funSuccess 
FunctionEnd 

; 安装失败 
Function funFail 
  StrCpy $My_FINISHPAGE_TEXT ${SAG_EN_PRODUCT_NAME}安装失败... 
FunctionEnd 
</code></pre>

## 方法二：根据某个文件是否存在判断是否成功 
### 在Section相关位置修改： 
<pre><code>
  ExecWait "$PROGRAMFILES\${SAG_EN_PRODUCT_NAME}\Tempbin\${SAG_FILE_PREFIX}WebBrowserDemo.exe" 
  call funFinish 
</code></pre>
### 函数： 
<pre><code>
; 执行exe之后调用方法判断文件是否存在 
Function funFinish 
; 根据文件是否存在判断是否成功 
  IfFileExists $PROGRAMFILES\${SAG_EN_PRODUCT_NAME}\aa.exe 0 fail 
    ;MessageBox MB_OK "hi testaaa! $My_FINISHPAGE_TEXT" 
    Goto end 
  fail: 
    StrCpy $My_FINISHPAGE_TEXT ${SAG_EN_PRODUCT_NAME}安装失败aaa... 
    ;MessageBox MB_OK "hi testbbb! $My_FINISHPAGE_TEXT" 
  end: 
FunctionEnd 
</code></pre>

## 需要注意的地方
### define的顺序会影响结果
如下的顺序，如果MUI_FINISHPAGE_TEXT的define放到MUI_PAGE_FINISH后面，最后的界面的文字就是默认的文字了。
<pre><code>
!define MUI_FINISHPAGE_TEXT "$My_FINISHPAGE_TEXT" 
!insertmacro MUI_PAGE_FINISH
</code></pre>


