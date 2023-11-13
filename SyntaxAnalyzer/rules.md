# Грамматика языка

```
snl = [`new_line`]
separator = (`;`|`new_line`)
# Правила, которые будут оперделены позже
# expression
# stmt
# inline_stmt
# overloadable_operator

operator_overload = `operator`, overloadable_operator
conversion = `conversion`, `[`, `identifier`, `]`
attribute_name = (`identifier`|operator_overload|conversion)
attribute = expression, `.`, attribute_name
indexator = expression, `[`, snl, expression, snl, `]`

optional_final = [`final`]
identifier_with_final = optional_final, `identifier`
assignable = (attribute|indexator|identifier_with_final)
assignable_sequence = {assignable, `,`}
assign_operator = (`=`|`+=`|`-=`|`*=`|`/=`, `//=`, `%=`, `&=`, `|=`, `^=`, `<<=`, `>>=`, `&&=`, `||=`)
assign_statement = assignable, `,`, assignable_sequence, assign_operator, expression

parenth = `(`, snl, expression, snl, `)`
else_stmt = `else`, snl, stmt
optional_else = [else_stmt]
if_stmt = `if`, snl, parenth, snl, stmt, optional_else

while_stmt = `while`, snl, parenth, snl, stmt

optional_inline_stmt = [inline_stmt]
optional_expr = [expression]
for_stmt = `for`, snl, `(`, snl, optional_inline_stmt, snl, `;`, snl, optional_expr, snl, `;`, snl, optional_inline_stmt, snl, `)`, snl, stmt
foreach = `foreach`, snl, `(`, snl, `identifier`, snl, `:`, snl, expression, snl, `)`, snl, stmt

optional_identifier = [`identifier`]
catch_stmt = `catch`, snl, `(`, `identifier`, snl, optional_identifier, snl, `)`, snl, stmt
catch_sequence = {catch_stmt, separator}
finally_stmt = `finally`, snl, stmt
optional_finally = [finally_stmt]
catch_block = catch_stmt, separator, catch_sequence, optional_else, separator, optional_finally
try_body = (catch_block, finally)
try_stmt = `try`, snl, stmt, separator, try_body

break_stmt = `break`
continue_stmt = `continue`
return_stmt = `return`, expression
import_stmt = `import`, `string_literal`

field_stmt = optional_final, `field`, `identifier`

argument_with_value = `identifier`, `=`, expression
argument = (argument_with_value | `identifier`)
comma_with_new_line = `,`, snl
argument_sequence = {argument, comma_with_new_line}
function_arguments = `(`, snl, argument_sequence, snl, `)`
function_definition = `func`, attribute_name, snl, function_arguments, snl, stmt

superclasses_list = {`identifier`, comma_with_new_line}
additional_superclasses = `,`, snl, sperclass_list
optional_additional_superclasses = [additional_superclasses]
superclasses_declaration = `:`, snl, `identifier`, snl, optional_additional_superclasses
optional_superclasses_declaration = [superclasses_declaration]
class_type = (`class`, `interface`)
class_definition = class_type, `identifier`, snl, optional_superclasses_declaration, snl, stmt
```

