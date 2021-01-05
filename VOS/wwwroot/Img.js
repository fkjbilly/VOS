function ImageAutoZoom(Img) {
    var imgHtml = "<img src='" + Img.src + "' width='480px' height='480px'/>";
    layer.open({
        type: 1
        , title: '预览'
        , area: ["500px", "500px"]
        , id: 'Lay_Img'
        , shade: 0.6
        , maxmin: true
        , anim: 1
        , content: imgHtml
    });
}