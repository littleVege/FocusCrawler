-- Database: focus_house

-- DROP DATABASE focus_house;

CREATE DATABASE focus_house
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'C'
       LC_CTYPE = 'C'
       CONNECTION LIMIT = -1;

-- Table: comms

-- DROP TABLE comms;

CREATE TABLE comms
(
  cid integer NOT NULL,
  url text,
  built_at date,
  property_fee double precision,
  houses integer,
  parking integer,
  green_rate double precision,
  plot_ratio integer,
  region character varying(255),
  region_rank integer,
  traffic_rank integer,
  pm character varying(255),
  red character varying(255),
  assort_rate double precision
)
WITH (
  OIDS=FALSE
);
ALTER TABLE comms
  OWNER TO postgres;

-- Table: assorts

-- DROP TABLE assorts;

CREATE TABLE assorts
(
  cid integer NOT NULL,
  assort_id integer NOT NULL,
  distance integer,
  cost_time integer,
  type_id integer,
  lat double precision,
  lon double precision,
  assort_type_id integer,
  address character varying(255),
  price double precision,
  CONSTRAINT assorts_pkey PRIMARY KEY (cid, assort_id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE assorts
  OWNER TO postgres;


-- Table: history_prices

-- DROP TABLE history_prices;

CREATE TABLE history_prices
(
  cid integer NOT NULL,
  datetime date NOT NULL,
  price double precision,
  CONSTRAINT history_prices_pkey PRIMARY KEY (cid, datetime)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE history_prices
  OWNER TO postgres;


-- Table: raw_pages

-- DROP TABLE raw_pages;

CREATE TABLE raw_pages
(
  url text,
  crawl_time timestamp without time zone NOT NULL DEFAULT now(),
  response text NOT NULL,
  id bigserial NOT NULL,
  CONSTRAINT raw_pages_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE raw_pages
  OWNER TO postgres;
