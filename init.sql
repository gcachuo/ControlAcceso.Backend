create table users
(
    id              serial  primary key,
    address         varchar not null,
    phone_number    varchar not null,
    username        varchar not null,
    email           varchar not null,
    firstname       varchar not null,
    second_name     varchar,
    lastname        varchar not null,
    second_lastname varchar,
    password        varchar not null,
    constraint users_pk
        unique (address, phone_number)
);
create table roles
(
    id   serial
        constraint roles_pk
            primary key,
    name varchar not null
        constraint roles_pk_2
            unique
);
create table addresses
(
    id serial 
        constraint addresses_pk
            primary key,
    street varchar not null,
    number varchar not null,
    constraint unique_address
        unique (street, number)
);
