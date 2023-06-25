CREATE DATABASE weather_forecast;
CREATE DATABASE weather_forecast_test;

CREATE USER user_name; 

ALTER USER user_name 
    WITH ENCRYPTED PASSWORD 'user_password';

-- GRANT ALL PRIVILEGES ON ALL TABLES IN DATABASE weather_forecast TO user_name;
\c weather_forecast;

CREATE TABLE cities (
  city_id BIGSERIAL NOT NULL,
  name VARCHAR(100) NOT NULL,
  location POINT NOT NULL,
  PRIMARY KEY (city_id)
);

CREATE TABLE forecasts (
  city_id BIGSERIAL NOT NULL,
  forecast_date DATE NOT NULL,
  temperature FLOAT[],
  wind_speed FLOAT[],
  rain FLOAT[],
  snowfall FLOAT[],
  cloud_cover FLOAT[],
  PRIMARY KEY (city_id, forecast_date),
  CONSTRAINT fk_cities
    FOREIGN KEY (city_id)
      REFERENCES cities(city_id)
      ON DELETE CASCADE
);

INSERT INTO cities 
  (name, location) 
  VALUES ('Бишкек', '42.87,74.59');

\c weather_forecast_test;

CREATE TABLE cities (
  city_id BIGSERIAL NOT NULL,
  name VARCHAR(100) NOT NULL,
  location POINT NOT NULL,
  PRIMARY KEY (city_id)
);

CREATE TABLE forecasts (
  city_id BIGSERIAL NOT NULL,
  forecast_date DATE NOT NULL,
  temperature FLOAT[],
  wind_speed FLOAT[],
  rain FLOAT[],
  snowfall FLOAT[],
  cloud_cover FLOAT[],
  PRIMARY KEY (city_id, forecast_date),
  CONSTRAINT fk_cities
    FOREIGN KEY (city_id)
      REFERENCES cities(city_id)
      ON DELETE CASCADE
);

GRANT ALL ON ALL TABLES IN SCHEMA public TO user_name;
GRANT ALL ON ALL SEQUENCES IN SCHEMA public TO user_name;
