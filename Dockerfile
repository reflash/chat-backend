FROM fsharp
ADD . .
RUN sh build.sh
EXPOSE 8080
CMD ["run.sh"]