using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MobileAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cafeteria_menus",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cafeteria_menus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gps_coordinate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gps_coordinate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schools",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    website = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schools", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_preferences",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_preferences", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cafeteria_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    icon_name = table.Column<string>(type: "text", nullable: false),
                    cafeteria_menu_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cafeteria_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_cafeteria_item_cafeteria_menus_cafeteria_menu_id",
                        column: x => x.cafeteria_menu_id,
                        principalTable: "cafeteria_menus",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gps_coordinate_id = table.Column<int>(type: "integer", nullable: true),
                    street_address1 = table.Column<string>(type: "text", nullable: false),
                    street_address2 = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_address_gps_coordinate_gps_coordinate_id",
                        column: x => x.gps_coordinate_id,
                        principalTable: "gps_coordinate",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_announcements", x => x.id);
                    table.ForeignKey(
                        name: "fk_announcements_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "school_news",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    headline = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    date_published = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    author = table.Column<string>(type: "text", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_school_news", x => x.id);
                    table.ForeignKey(
                        name: "fk_school_news_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tutoring_centers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tutoring_centers", x => x.id);
                    table.ForeignKey(
                        name: "fk_tutoring_centers_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "campuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campuses", x => x.id);
                    table.ForeignKey(
                        name: "fk_campuses_address_address_id",
                        column: x => x.address_id,
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_campuses_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "business_offices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_business_offices", x => x.id);
                    table.ForeignKey(
                        name: "fk_business_offices_campuses_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "librarys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_librarys", x => x.id);
                    table.ForeignKey(
                        name: "fk_librarys_campuses_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "registrars",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_registrars", x => x.id);
                    table.ForeignKey(
                        name: "fk_registrars_campuses_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "school_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    school_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_school_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_school_events_campuses_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_school_events_schools_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "securitys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    office_location = table.Column<string>(type: "text", nullable: false),
                    contact_number = table.Column<string>(type: "text", nullable: false),
                    emergency_number = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_securitys", x => x.id);
                    table.ForeignKey(
                        name: "fk_securitys_campuses_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hours_of_operation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    day = table.Column<string>(type: "text", nullable: false),
                    open_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    close_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    business_office_id = table.Column<int>(type: "integer", nullable: true),
                    library_id = table.Column<int>(type: "integer", nullable: true),
                    registrar_id = table.Column<int>(type: "integer", nullable: true),
                    security_id = table.Column<int>(type: "integer", nullable: true),
                    tutoring_center_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hours_of_operation", x => x.id);
                    table.ForeignKey(
                        name: "fk_hours_of_operation_business_offices_business_office_id",
                        column: x => x.business_office_id,
                        principalTable: "business_offices",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_hours_of_operation_librarys_library_id",
                        column: x => x.library_id,
                        principalTable: "librarys",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_hours_of_operation_registrars_registrar_id",
                        column: x => x.registrar_id,
                        principalTable: "registrars",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_hours_of_operation_securitys_security_id",
                        column: x => x.security_id,
                        principalTable: "securitys",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_hours_of_operation_tutoring_centers_tutoring_center_id",
                        column: x => x.tutoring_center_id,
                        principalTable: "tutoring_centers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "security_alerts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    alert_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    security_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_security_alerts", x => x.id);
                    table.ForeignKey(
                        name: "fk_security_alerts_securitys_security_id",
                        column: x => x.security_id,
                        principalTable: "securitys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_address_gps_coordinate_id",
                table: "address",
                column: "gps_coordinate_id");

            migrationBuilder.CreateIndex(
                name: "ix_announcements_school_id",
                table: "announcements",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_business_offices_campus_id",
                table: "business_offices",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_cafeteria_item_cafeteria_menu_id",
                table: "cafeteria_item",
                column: "cafeteria_menu_id");

            migrationBuilder.CreateIndex(
                name: "ix_campuses_address_id",
                table: "campuses",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_campuses_school_id",
                table: "campuses",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_hours_of_operation_business_office_id",
                table: "hours_of_operation",
                column: "business_office_id");

            migrationBuilder.CreateIndex(
                name: "ix_hours_of_operation_library_id",
                table: "hours_of_operation",
                column: "library_id");

            migrationBuilder.CreateIndex(
                name: "ix_hours_of_operation_registrar_id",
                table: "hours_of_operation",
                column: "registrar_id");

            migrationBuilder.CreateIndex(
                name: "ix_hours_of_operation_security_id",
                table: "hours_of_operation",
                column: "security_id");

            migrationBuilder.CreateIndex(
                name: "ix_hours_of_operation_tutoring_center_id",
                table: "hours_of_operation",
                column: "tutoring_center_id");

            migrationBuilder.CreateIndex(
                name: "ix_librarys_campus_id",
                table: "librarys",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_registrars_campus_id",
                table: "registrars",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_school_events_campus_id",
                table: "school_events",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_school_events_school_id",
                table: "school_events",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_school_news_school_id",
                table: "school_news",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_security_alerts_security_id",
                table: "security_alerts",
                column: "security_id");

            migrationBuilder.CreateIndex(
                name: "ix_securitys_campus_id",
                table: "securitys",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_tutoring_centers_school_id",
                table: "tutoring_centers",
                column: "school_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "cafeteria_item");

            migrationBuilder.DropTable(
                name: "hours_of_operation");

            migrationBuilder.DropTable(
                name: "school_events");

            migrationBuilder.DropTable(
                name: "school_news");

            migrationBuilder.DropTable(
                name: "security_alerts");

            migrationBuilder.DropTable(
                name: "user_preferences");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "cafeteria_menus");

            migrationBuilder.DropTable(
                name: "business_offices");

            migrationBuilder.DropTable(
                name: "librarys");

            migrationBuilder.DropTable(
                name: "registrars");

            migrationBuilder.DropTable(
                name: "tutoring_centers");

            migrationBuilder.DropTable(
                name: "securitys");

            migrationBuilder.DropTable(
                name: "campuses");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "schools");

            migrationBuilder.DropTable(
                name: "gps_coordinate");
        }
    }
}
