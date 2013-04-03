dim fso
dim version
dim file
dim content
dim stream
dim revision

year1 = right(Year(Date),2)

month1 = Month(Date)
if Len(month1) < 2 then
month1 = "0" & month1
end if

day1 = Day(Date) 
if Len(day1) < 2 then
day1 = "0" & day1
end if



revision = month1 & day1

Set fso = CreateObject("Scripting.FileSystemObject")

'get version
set file = fso.OpenTextFile("version.txt", 1, true, 0)
version = file.ReadLine() & "." & revision
file.Close()


'remove file readonly attribute
set file = fso.GetFile("CMSAssemblyInfoGlobal.cs")
file.Attributes =  file.Attributes or 1

'get old content
set stream =  CreateObject("ADODB.Stream")
stream.Type = 2
stream.Charset = "UTF-8"
stream.Open() 
stream.LoadFromFile("CMSAssemblyInfoGlobal.cs") 
content = stream.ReadText()
stream.Close()   
    
'update content
content = left(content, instr(content, "[assembly: AssemblyVersion(""") - 1) &_
    "[assembly: AssemblyVersion(""" & version & """)]" & Chr(13) & Chr(10) &_
    "[assembly: AssemblyFileVersion("""  & version & """)]" & Chr(13) & Chr(10)
    
'delete old file 
file.Delete(true)
    
'save back
stream.Open()
stream.WriteText(content) 
stream.SaveToFile "CMSAssemblyInfoGlobal.cs", 2
stream.Close() 
