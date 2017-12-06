#! /usr/bin/python -u

# not really a packer but attempt to reduce file size by resizing assets
def pack_binary(original, destination):

    fd = open(original, "rb")
    original_binary = fd.read()
    fd.close()

    # loading assets boundaries
    offsets = {}
    orders = []
    for t in ["TECH", "NORMAL", "L1", "L2", "L3"]:
        filename = "WindowsApplication1._%s.bin" % t
        print 'reading', filename
        fd = open(filename, "rb")
        asset = fd.read()
        fd.close()

        beginning = asset[:16]
        end = asset[len(asset)-17:]
        offset = 0
        while True:
            offset = original_binary.find(beginning, offset+1)
            if offset == -1:
                break
            start_offset = offset
            offset = original_binary.find(end, offset+1)
            if offset == -1:
                break
            end_offset = offset
#            print 'size of asset in binary:', (end_offset + 16 + 1) - start_offset
            offset_obj = {"start": start_offset, "end": end_offset + 16 + 1, "data": beginning, "len": len(beginning), "size": len(asset) }
#            print offset_obj
            offsets[start_offset] = offset_obj
            orders.append(start_offset)

    #print offsets
    orders.sort()
    offset = 0
    newbinary = ""
    lastoffset = False
    for i in orders:
        off = offsets[i]
        print i, off
        if lastoffset is False:
            lastoffset = 0
        newbinary += original_binary[lastoffset:off["start"]]
        newbinary += off["data"]
        newbinary += '\0' * (off["size"] - 16)
        lastoffset = off["end"]
    newbinary += original_binary[lastoffset:]


    print 'new binary size:', len(newbinary)

    fd = open(destination, "wb")
    fd.write(newbinary)
    fd.close()

pack_binary("ZAR.exe", "ZAR.packed.exe")

