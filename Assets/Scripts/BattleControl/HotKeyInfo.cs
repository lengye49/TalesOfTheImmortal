public class HotKeyInfo {
    public int id;
    public int type;//0技能|1物品
    public int param;
    public bool IsInteractive;
    public HotKeyInfo(){
        id = 0;
        type = 0;
        param = 1;
        IsInteractive = true;
    }

}
