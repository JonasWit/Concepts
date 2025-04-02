﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DemoApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoApi.Migrations
{
    [DbContext(typeof(BookstoreDbContext))]
    [Migration("20240926143128_PublisherKey")]
    partial class PublisherKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Demo.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "Demo.Models.Author.Name#PersonalName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("First")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("Last")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("LastName");

                            b1.Property<string>("Middle")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("MiddleNames");
                        });

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Authors", (string)null);
                });

            modelBuilder.Entity("Demo.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PublisherId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ComplexProperty<Dictionary<string, object>>("Release", "Demo.Models.Book.Release#Release", b1 =>
                        {
                            b1.IsRequired();

                            b1.ComplexProperty<Dictionary<string, object>>("EditionRepresentation", "Demo.Models.Book.Release#Release.EditionRepresentation#EditionRepresentation", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Discriminator")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(15)")
                                        .HasColumnName("EditionDiscriminator");

                                    b2.Property<int>("Number")
                                        .HasColumnType("int")
                                        .HasColumnName("EditionNumber");

                                    b2.Property<string>("Season")
                                        .HasColumnType("nvarchar(6)")
                                        .HasColumnName("EditionSeason");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("PublicationRepresentation", "Demo.Models.Book.Release#Release.PublicationRepresentation#PublicationInfoRepresentation", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<int?>("Date")
                                        .HasColumnType("int")
                                        .HasColumnName("PublicationDate");

                                    b2.Property<string>("Discriminator")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(13)")
                                        .HasColumnName("PublicationDiscriminator");
                                });
                        });

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.HasIndex("PublisherId");

                    b.ToTable("Books", (string)null);
                });

            modelBuilder.Entity("Demo.Models.BookAuthor", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int");

                    b.HasKey("BookId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.ToTable("BookAuthors", (string)null);
                });

            modelBuilder.Entity("Demo.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Publishers", (string)null);
                });

            modelBuilder.Entity("Demo.Models.Book", b =>
                {
                    b.HasOne("Demo.Models.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("Demo.Models.BookAuthor", b =>
                {
                    b.HasOne("Demo.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Demo.Models.Book", "Book")
                        .WithMany("AuthorsCollection")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Demo.Models.Book", b =>
                {
                    b.Navigation("AuthorsCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
