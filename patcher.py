#! /usr/bin/python -u

import hashlib
import random

def extract_from(filename):
    fd = open(filename, "rb")
    data = fd.read(16)
    fd.close()
    return data

def extract_random(filename):
    fd = open(filename, "rb")
    data = fd.read()
    fd.close()
    max = len(data) / 16
    off = random.randint(0, max) * 16
    serial = ""
    print 'using serial stored at 0x%s in %s' % (off, filename)
    for i in range(16):
        serial += data[off+i]
    return serial

old_hash = extract_random("WindowsApplication1._TECH.bin")

def patch_binary(binary, hashed, filename):
    fd = open(filename, "rb")
    data = fd.read()
    fd.close()

    offsets = []
    offset = 0
    oldoffset = 0
    while True:
        offset = data.find(binary, offset+1)
        if offset == -1:
            break
        offsets.append(offset)
        old_offset = offset + 16

    copy = data
    for offset in offsets:
        print 'patching offset 0x%d' % offset
        newbinary = data[:offset]
        newbinary += hashed
        newbinary += data[offset+16:]
        copy = newbinary

    fd = open("%s.cracked.exe" % filename.replace(".exe", ""), "wb")
    fd.write(newbinary)
    fd.close()
    print "patch produced at %s.cracked.exe" % filename

serial = "AAAAA-AAAAA-AAAAA-AAAAA-AAAAA"
hashed = hashlib.md5(serial).digest()

patch_binary(old_hash, hashed, "ZAR.exe")

