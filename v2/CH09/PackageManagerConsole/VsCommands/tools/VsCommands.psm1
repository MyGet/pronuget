function Remove-Usings {
    $dte.ExecuteCommand("ProjectandSolutionContextMenus.Project.RemoveandSortUsings")
}

function Find-Symbol {
    param(
        [parameter(ValueFromPipelineByPropertyName = $true)]
        [string]$Name
    )

    $dte.ExecuteCommand("Edit.FindSymbol", $Name)
}

Export-ModuleMember -Function *

