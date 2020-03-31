#!/bin/bash
######################################
#  How to use:
#    - Set the project name
#    - run $ bash local_publish.sh
#
######################################

# Set Variables
project_name="Vra.XrmToolbox.Installer"

# Static vars
plugins_location="$APPDATA/MscrmTools/XrmToolBox/Plugins/"
dll_location="$project_name/bin/Debug/$project_name.dll"
solution_name="$project_name/$project_name.csproj"

# Do the work
rm -rf `find -type d -name obj`
rm -rf `find -type d -name bin`
dotnet build $solution_name
copy_error=$(cp $dll_location $plugins_location 2>&1)


# Give information if error occured while putting the dll in the XrmToolbox dlls location
if [[ $copy_error == *"Device or resource busy"* ]]
then
	echo -e "\n\n\n\n\n\n#####################\n\n"
	echo "  ERROR: Unable to copy the new dll to the location used by XrmToolbox."
	echo "    - You forgot to close the XrmToolbox application. Please make sure you closed it befor running this task. Close it & rerun the script again."
	echo -e "\n\n#####################"
else
	echo "Script ran successfully. Start up your XrmToolbox to start using the latest edition!"
fi


