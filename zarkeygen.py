#! /usr/bin/python -u

import hashlib
import random

class ZAR_Keygen():
    def __init__(self):
        self.md5 = hashlib.md5()
        self._random_init = False
        self.serials = {}
        self.md5s = {}
        self.used = {}
        self.success = {}
        for t in ["TECH", "NORMAL", "L1", "L2", "L3"]:
            filename = "WindowsApplication1._%s.bin" % t
            self.load(filename, t)
#        import json
#        print json.dumps(self.serials, indent=2)

    # load databases of md5 stored 
    def load(self, filename, licensetype):
        fd = open(filename)
        data = fd.read()
        fd.close()

        print "loading %d serials from %s" % (len(data) / 16, filename)
        for i in range(len(data) / 16):
            string = data[i*16:(i+1)*16]

            self.serials[string] = licensetype
            continue # skip doubling the string
            serial = ""
            for l in string:
                serial += "%02x" % ord(l)
#            print "serial found:", serial, len(serial)
            self.serials[serial] = licensetype

    # load already used generated serials on disk
    def load_used(self):
        pass

    def write(self, serial, licensetype):
        fd = open("serials.txt", "a+")
        fd.write("%s %s\n" % (serial, licensetype))
        fd.close()

    def generate_key(self):
        serial = ""
        for i in range(5):
            for j in range(5):
                serial += self.chars[random.randint(0, self.chars_length)]
            if i != 4:
                serial += "-"
        return serial

    def hash(self, serial):
        return hashlib.md5(serial).digest()
#        return hashlib.md5(serial).hexdigest()

    def _gen_random_init(self):
        if self._random_init is False:
            self.chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            self.chars_length = len(self.chars) - 1
            self._random_init = True

    def gen_random(self):
        self._gen_random_init()
        used = True
        while used:
            serial = self.generate_key()
            if not serial in self.used:
                used = False
        return serial

    def bruteforce(self):
        import time
        print 'starting bruteforce with', len(self.serials), 'serials loaded'
        self._gen_random_init()
        count = 0
        found = 0
        start = time.time()
        while True:
            if not (count % 50000):
                diff = time.time() - start
                print "generated %10d serials %3d found (took %.02f sec)" % (count, found, diff)
                start = time.time()

            count += 1
            #serial = self.gen_random()
            # bypass memory storage of used serials:
            serial = self.generate_key()
#            continue
            h = self.hash(serial)
            #print serial, h
            if h in self.serials:
                found += 1
                print 'found', h, 'in serials!', serial, 'licensetype:', self.serials[h]
                self.write(serial, self.serials[h])
            #self.used[serial] = 1


keygen = ZAR_Keygen()
keygen.bruteforce()



