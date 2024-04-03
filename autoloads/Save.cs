using Godot;
using System;
using Godot.Collections;

public partial class Save : Node{
    const string SavePath="user://saves/";
    const string SaveFileName="file.json";
    const string SecurityKey="871SVEMR";
    RomaGay Roma;
    PlayerStats StatsS;

    public override void _Ready(){
        Roma=GetNode<RomaGay>("/root/RomaGay");
        StatsS=new PlayerStats();
        VerifySaveDirectory(SavePath);
    }
    private void VerifySaveDirectory(String path){
        DirAccess.MakeDirAbsolute(path);
    }
    public void Saving(){ 
       using var file= FileAccess.OpenEncryptedWithPass(SavePath+SaveFileName,FileAccess.ModeFlags.Write,SecurityKey);
        if (file==null){
            GD.Print(FileAccess.GetOpenError());
            return;
        }
       var data = new Dictionary<string, Variant>
        {
            {"StatsS", new Dictionary<string,Variant>(){       
                {"Position", new Dictionary<string, Variant>
                    {
                        { "X", Roma.statsG.pos.X},
                        { "Y", Roma.statsG.pos.Y}
                    }
                },
                {"Lives", Roma.statsG.Lives}, 
                {"Played", Roma.statsG.Played},
                {"Level", Roma.statsG.pathLevel}
        }}};
        var JsonString=Json.Stringify(data,"\t");
        file.StoreString(JsonString);
        file.Close();
    }
    public void Load(){
        if(FileAccess.FileExists(SavePath+SaveFileName)){
            
            using var file= FileAccess.OpenEncryptedWithPass(SavePath+SaveFileName,FileAccess.ModeFlags.Read,SecurityKey);
           
            if (file==null){
                GD.Print(FileAccess.GetOpenError());
                return;
            }
            var content = file.GetAsText();
             file.Close();
              
            var data= Json.ParseString(content);
            if(data.Equals(null)){
                GD.PrintErr($"Cannot parse {SavePath+SaveFileName} as a JsonString: {content}!");
                return;
            } 
            var _Statss= (Dictionary)data;
            var _StatsS=(Dictionary)_Statss["StatsS"];
            Roma.statsG.Lives=(int)_StatsS["Lives"];
            Roma.statsG.Played=(bool)_StatsS["Played"];
            Roma.statsG.pathLevel=(string)_StatsS["Level"];
            var _StatsSpos=(Dictionary)_StatsS["Position"];
            Roma.statsG.pos=new Vector2((float)_StatsSpos["X"],(float)_StatsSpos["Y"]);
        }else{
            GD.PrintErr($"Cannot open non-existent file at {SavePath+SaveFileName}!");
        }
    }
}
