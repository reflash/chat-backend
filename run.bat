@echo off
cls

docker build -t reflash/chat .
docker run -d -p 127.0.0.1:80:4567 reflash/chat /bin/sh -c "cd /root/chat; bundle exec foreman start;"
docker ps -a
docker run reflash/chat /bin/sh -c "cd /root/chat; bundle exec rake test"
