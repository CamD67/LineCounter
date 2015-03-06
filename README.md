# LineCounter
Counts significant lines in code.
Significant lines are lines that aren't curly braces, import statements, class definitions, etc.
This isn't a complex project, just wanted a tool to count up lines of code.
I'm also aware that "lines of code" isn't a very good metric to judge code complexity

#Supported Languages
*C#
*Java
*Possibly others but no guarantees

#Bugs and other issues
*Misses block comments that aren't preceded by '*'s
*Off by 1 on tested code. Possibly not removing class headers properly or end of files