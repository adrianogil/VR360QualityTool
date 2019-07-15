
target_metrics="MSE SSIM PSNR"

function image_compare()
{
    direction=$1
    image_type=$2

    img_ref=unity/VR360QualityTool/Screenshots/Screenshot_${direction}_Skybox.jpg
    img_cmp=unity/VR360QualityTool/Screenshots/Screenshot_${direction}_${image_type}.jpg
    chart_path=charts/Direction${direction}_${image_type}.png
    python3 python/qualityassessment.py  ${img_ref} ${img_cmp} --graph --report ${chart_path} ${target_metrics}
}

image_compare 0 Equi2Cube
image_compare 0 Sphere
image_compare 2 Equi2Cube
image_compare 2 Sphere
image_compare 5 Equi2Cube
image_compare 5 Sphere
