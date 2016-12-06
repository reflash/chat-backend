# Chat backend in F#

Server installation scripts
```sh
# update current packages
$ sudo apt-get update
$ sudo apt-get install apt-transport-https ca-certificates

# add new GPG key
$ sudo apt-key adv \
               --keyserver hkp://ha.pool.sks-keyservers.net:80 \
               --recv-keys 58118E89F3A912897C070ADBF76221572C52609D


$ echo "<REPO>" | sudo tee /etc/apt/sources.list.d/docker.list
$ sudo apt-get update

$ apt-cache policy docker-engine

  docker-engine:
    Installed: 1.12.2-0~trusty
    Candidate: 1.12.2-0~trusty
    Version table:
   *** 1.12.2-0~trusty 0
          500 https://apt.dockerproject.org/repo/ ubuntu-trusty/main amd64 Packages
          100 /var/lib/dpkg/status
       1.12.1-0~trusty 0
          500 https://apt.dockerproject.org/repo/ ubuntu-trusty/main amd64 Packages
       1.12.0-0~trusty 0
          500 https://apt.dockerproject.org/repo/ ubuntu-trusty/main amd64 Packages

# Recommended packages
$ sudo apt-get install linux-image-extra-$(uname -r) linux-image-extra-virtual

# Docker installation



```
