/**
 * 图片显示
 * @param {any} img
 */
function ImageAuto(src) {
    let _src;
    if (src.indexOf("base64") < 0) {
        _src = src.replace(/64/g, "640");
    } else {
        _src = src;
    }
    var imgHtml = "<img src='" + _src + "' width='640px' height='640px'/>";
    parent.layer.open({
        type: 1
        , title: '预览'
        , area: ["640px", "640px"]
        , id: "img"
        , shade: 0.6
        , maxmin: true
        , anim: 1
        , content: imgHtml
    });
}