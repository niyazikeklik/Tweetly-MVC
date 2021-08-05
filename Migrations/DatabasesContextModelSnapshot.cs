﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tweetly_MVC.Models;

namespace Tweetly_MVC.Migrations
{
    [DbContext(typeof(DatabasesContext))]
    partial class DatabasesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tweetly_MVC.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BegeniSikligi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cinsiyet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .UseIdentityColumn().UseIdentityColumn(1,1);
                    

                    b.Property<double>("Date")
                        .HasColumnType("float");

                    b.Property<string>("FollowStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Followers")
                        .HasColumnType("int");

                    b.Property<string>("FollowersStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Following")
                        .HasColumnType("int");

                    b.Property<double>("LastLikesDate")
                        .HasColumnType("float");

                    b.Property<double>("LastTweetsDate")
                        .HasColumnType("float");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TweetCount")
                        .HasColumnType("int");

                    b.Property<string>("TweetSikligi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isPrivate")
                        .HasColumnType("bit");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
