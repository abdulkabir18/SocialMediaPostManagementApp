create table if not exists Users
(
    Id char(36) Primary Key not null unique,
    Email varchar(250) not null unique,
    Password varchar(250) not null unique,
    Role varchar(25) not null,
    IsDelete varchar(20) not null
);
create table if not exists SocialMediaUsers
(
    Id char(36) Primary key not null unique,
    FirstName varchar(120) not null,
    LastName varchar(120) not null,
    UserName varchar(100) null unique,
    Email varchar(250) not null unique,
    Gender varchar(20) not null,
    Address varchar(250) null,
    DateOfBirth Date not null,
    IsDelete varchar(20) not null
);
create table if not exists Posts
(
    Id char(36) Primary key not null unique,
    Title varchar(150) null,
    Content varchar(300) not null,
    SocialMediaUserId char(36) not null,
    DateCreated DATETIME not null,
    CreatedBy varchar(200) not null,
    Foreign Key(SocialMediaUserId) references SocialMediaUsers(Id)
);
create table if not exists Likes
(
    Id char(36) Primary key not null unique,
    PostId char(150) not null,
    DateLiked DATETIME not null,
    LikedBy varchar(200) not null,
    Foreign Key(PostId) references Posts(Id)
);
create table if not exists Comments
(
    Id char(36) Primary key not null unique,
    PostId char(150) not null,
    Message varchar(200) null,
    DateComment DATETIME not null,
    CommentBy varchar(200) not null,
    Foreign Key(PostId) references Posts(Id)
);
create table if not exists Replies
(
    Id char(36) Primary key not null unique,
    CommentId char(150) not null,
    Message varchar(300) null,
    DateReply DATETIME not null,
    ReplyBy varchar(200) not null,
    MediaUserId char(150) not null,
    Foreign Key(CommentId) references Comments(Id),
    Foreign Key(MediaUserId) references SocialMediaUsers(Id)
);