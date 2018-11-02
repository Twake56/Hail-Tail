import time

num = 1
while True:
    with open('C:\\Users\\twakeman\\Documents\\GitHub\\Timber-Tail/LogGenerator/test.log', "a+") as log:
        log.write('New logline' + str(num) + '\n')
        num += 1
        log.close()
        time.sleep(1)
