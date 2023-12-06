namespace DeadCells.Core;

public class DCAnimPic
{
    /// <summary>
    /// 两部分组成，动作名称_序号
    /// </summary>
    public string name;
    /// <summary>
    ///重新利用。表示是哪一张图片。0就是beheadedModHelper0.png 
    /// </summary>
    public int index;
    /// <summary>
    /// 贴图左上角在大图里面的横坐标。
    /// </summary>
    public int x;
    /// <summary>
    /// 贴图左上角在大图里面的纵坐标。
    /// </summary>
    public int y;
    /// <summary>
    /// 贴图宽。
    /// </summary>
    public int width;
    /// <summary>
    /// 贴图高。
    /// </summary>
    public int height;

    //下面的用来算绘制位置。

    //贴图左上角相对放进的新图的左上角的水平偏移
    public int offsetX;
    //贴图左上角相对放进的新图的左上角的垂直偏移
    public int offsetY;
    //贴图裁出来后放进的新图的宽（如150×150）
    public int originalWidth;
    //贴图裁出来后放进的新图的高（如150×150）
    public int originalHeight;
}
