FROM python:alpine

# RUN apk --no-cache add --virtual .builddeps gcc gfortran musl-dev     && pip install numpy==1.14.0     && apk del .builddeps     && rm -rf /root/.cache

RUN apk --update add --virtual .builddeps gcc build-base openblas-dev gfortran gfortran musl-dev
# freetype-dev libpng-dev

RUN pip install --upgrade setuptools
RUN pip install numpy
RUN pip install pandas
# RUN pip install matplotlib
RUN pip install pystan
RUN pip install fbprophet

RUN apk del .builddeps
RUN rm -rf /root/.cache

# RUN mkdir /app
WORKDIR /app
COPY src/* ./

ENTRYPOINT [ "python" "main.py" ]