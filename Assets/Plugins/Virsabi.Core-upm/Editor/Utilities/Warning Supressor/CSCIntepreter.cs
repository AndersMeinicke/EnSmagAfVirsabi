using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Virsabi.Internal;

public class CSCIntepreter : Editor
{
    private void OnWizardCreate()
    {
        //if (AllApproved())
            WriteSettings();
    }

    public static void WriteSettings()
    {
        Debug.Log("Writing ignore rules to" + Application.dataPath + "/csc.rsp");
        
        using (StreamWriter writer = new StreamWriter(Application.dataPath + "/csc.rsp"))
        {
            foreach (VirsabiSettings.SupressDefinition item in VirsabiSettings.WarningSupresisons)
            {
                if (item.Enabled)
                    writer.WriteLine("-nowarn:" + item.WarningCode);
            }
        }

        AssetDatabase.Refresh();
    }
}
