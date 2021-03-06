#!/usr/bin/python

from skimage.measure import compare_ssim as ssim

import matplotlib
matplotlib.use('TkAgg')
import matplotlib.pyplot as plt

import numpy as np
import cv2

# https://www.pyimagesearch.com/2014/09/15/python-compare-two-images/
def mse(imageA, imageB):
    # the 'Mean Squared Error' between the two images is the
    # sum of the squared difference between the two images;
    # NOTE: the two images must have the same dimension
    err = np.sum((imageA.astype("float") - imageB.astype("float")) ** 2)
    err /= float(imageA.shape[0] * imageA.shape[1])

    # return the MSE, the lower the error, the more "similar"
    # the two images are
    return err

def psnr(imageA, imageB):
    mse_value = mse(imageA, imageB)
    if mse_value == 0:
        return 100
    PIXEL_MAX = 255.0
    return 20 * np.log10(PIXEL_MAX / np.sqrt(mse_value))

def compare_images(imageA, imageB, results_path, title, chart_path, metrics):
    # compute the mean squared error and structural similarity
    # index for the images

    # setup the figure
    # ax.plot([0,1,2], [10,20,3])
    title = ""
    fig = plt.figure(title)

    metrics_str = ""
    metrics_data = ()

    if "MSE" in metrics:
        if metrics_str == "":
            metrics_str = "MSE: %.2f"
        else:
            metrics_str = metrics_str + ", MSE: %.2f"
        # compute the mean squared error
        metrics_data = metrics_data + (mse(imageA, imageB),)

    if "SSIM" in metrics:
        if metrics_str == "":
            metrics_str = "SSIM: %.2f"
        else:
            metrics_str = metrics_str + ", SSIM: %.2f"
        # compute the structural similarity index
        metrics_data = metrics_data + (ssim(imageA, imageB),)

    if "PSNR" in metrics:
        if metrics_str == "":
            metrics_str = "PSNR: %.2f"
        else:
            metrics_str = metrics_str + ", PSNR: %.2f"
        # compute the PSNR
        metrics_data = metrics_data + (psnr(imageA, imageB),)

    if "WSPSNR" in metrics:
        if metrics_str == "":
            metrics_str = "WS-PSNR: %.2f"
        else:
            metrics_str = metrics_str + ", WS-PSNR: %.2f"
        # compute the WS-PSNR
        metrics_data = metrics_data + (psnr(imageA, imageB),)

    print(metrics_str % metrics_data)
    print()

    plt.suptitle(metrics_str % metrics_data)

    # show first image
    ax = fig.add_subplot(1, 2, 1)
    plt.imshow(imageA, cmap = plt.cm.gray)
    plt.axis("off")

    # show the second image
    ax = fig.add_subplot(1, 2, 2)
    plt.imshow(imageB, cmap = plt.cm.gray)
    plt.axis("off")

    # show the images
    fig.savefig(chart_path)   # save the figure to file
    # plt.show()

def process_images(imageA_path, imageB_path, results_path, is_show_graph, is_build_report, chart_path, metrics):

    print("Reference image: %s" % (imageA_path,))
    print("Target image: %s" % (imageB_path,))

    original = cv2.imread(imageA_path)
    original_degraded = cv2.imread(imageB_path)

    # convert the images to grayscale
    original = cv2.cvtColor(original, cv2.COLOR_BGR2GRAY)
    original_degraded = cv2.cvtColor(original_degraded, cv2.COLOR_BGR2GRAY)

    compare_images(original, original_degraded, results_path, 'Images', chart_path, metrics)

if __name__ == '__main__':
    import sys

    if len(sys.argv) == 1:
        # load the images -- the original, the original + contrast,
        # and the original + photoshop
        process_images("images/ballons.jpg", "images/ballons_degraded.jpg", '../Results/comparison.png', 'chart.png', ["MSE", "SSIM"])

    elif len(sys.argv) > 7:
        metrics = []

        for m in range(6, len(sys.argv)):
            metrics.append(sys.argv[m])

        is_show_graph = (sys.argv[4] == '--graph')
        is_build_report = (sys.argv[5] == '--report')

        chart_path = sys.argv[6]

        process_images(sys.argv[1], sys.argv[2], sys.argv[3], is_show_graph, is_build_report, chart_path, metrics)
