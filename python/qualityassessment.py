#!/usr/bin/python

from skimage.measure import structural_similarity as ssim
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

def compare_images(imageA, imageB, title, metrics):
    # compute the mean squared error and structural similarity
    # index for the images

    # setup the figure
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
    plt.show()

def process_images(imageA_path, imageB_path, metrics):
    original = cv2.imread(imageA_path)
    original_degraded = cv2.imread(imageB_path)

    # convert the images to grayscale
    original = cv2.cvtColor(original, cv2.COLOR_BGR2GRAY)
    original_degraded = cv2.cvtColor(original_degraded, cv2.COLOR_BGR2GRAY)

    compare_images(original, original_degraded, 'Images', metrics)

if __name__ == '__main__':
    import sys

    if len(sys.argv) == 1:
        # load the images -- the original, the original + contrast,
        # and the original + photoshop
        process_images("images/ballons.jpg", "images/ballons_degraded.jpg", ["MSE", "SSIM"])

    elif len(sys.argv) > 3:
        metrics = []

        for m in xrange(3, len(sys.argv)):
            metrics.append(m)

        process_images(sys.argv[1], sys.argv[2], metrics)
