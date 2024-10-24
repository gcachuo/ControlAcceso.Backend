﻿create table users
(
    id              serial
        primary key,
    address         varchar not null,
    phone_number    varchar not null,
    username        varchar not null,
    email           varchar not null,
    firstname       varchar not null,
    second_name     varchar,
    lastname        varchar not null,
    second_lastname varchar,
    password        varchar not null,
    role_id         integer not null
        constraint users_roles_id_fk
            references roles
            on update cascade on delete cascade,
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
    id     serial  not null
        constraint addresses_pk
            primary key,
    street varchar not null,
    number varchar not null,
    constraint unique_address
        unique (street, number)
);
create table refresh_tokens
(
    id           serial
        primary key,
    token        text      not null,
    user_id      integer   not null
        constraint fk_user
            references users,
    created_at   timestamp not null,
    expires_at   timestamp not null,
    is_valid     boolean default true,
    ip_address   varchar(45),
    user_agent   text,
    last_used_at timestamp
);