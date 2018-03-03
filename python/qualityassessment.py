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

def compare_images(imageA, imageB, title):
    # compute the mean squared error and structural similarity
    # index for the images
    m = mse(imageA, imageB)
    s = ssim(imageA, imageB)
 
    # setup the figure
    fig = plt.figure(title)
    plt.suptitle("MSE: %.2f, SSIM: %.2f" % (m, s))
 
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

if __name__ == '__main__':
    import sys

    if len(sys.argv) == 1:
        # load the images -- the original, the original + contrast,
        # and the original + photoshop
        original = cv2.imread("images/ballons.jpg")
        original_degraded = cv2.imread("images/ballons_degraded.jpg")

        # convert the images to grayscale
        original = cv2.cvtColor(original, cv2.COLOR_BGR2GRAY)
        original_degraded = cv2.cvtColor(original_degraded, cv2.COLOR_BGR2GRAY)

        compare_images(original, original_degraded, 'Images')