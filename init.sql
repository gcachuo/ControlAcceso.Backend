create table users
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