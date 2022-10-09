using Microsoft.EntityFrameworkCore.Migrations;

namespace MafiaOnline.DataAccess.Migrations
{
    public partial class AddedItalianFirstNamesAndLastNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Name",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Name", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Name",
                columns: new[] { "Id", "Text", "Type" },
                values: new object[,]
                {
                    { 1L, "Adele", 0 },
                    { 98L, "Tommaso", 0 },
                    { 99L, "Valerius", 0 },
                    { 100L, "Vincenzo", 0 },
                    { 101L, "Fasano", 1 },
                    { 102L, "Lo Iacono", 1 },
                    { 103L, "Montani", 1 },
                    { 104L, "Cerminaro", 1 },
                    { 105L, "Paganini", 1 },
                    { 106L, "Di Pinto", 1 },
                    { 107L, "La Fratta", 1 },
                    { 108L, "Antonelli", 1 },
                    { 109L, "Bellofatto", 1 },
                    { 110L, "Sama", 1 },
                    { 111L, "Virginia", 1 },
                    { 112L, "Rucci", 1 },
                    { 97L, "Thomas", 0 },
                    { 113L, "Schifano", 1 },
                    { 96L, "Stefano", 0 },
                    { 94L, "Samuel", 0 },
                    { 79L, "Lorenzo", 0 },
                    { 80L, "Luca", 0 },
                    { 81L, "Luigi", 0 },
                    { 82L, "Manuel", 0 },
                    { 83L, "Marco", 0 },
                    { 84L, "Matteo", 0 },
                    { 85L, "Mattia", 0 },
                    { 86L, "Michele", 0 },
                    { 87L, "Nathan", 0 },
                    { 88L, "Nicola", 0 },
                    { 89L, "Nicolo", 0 },
                    { 90L, "Pietro", 0 },
                    { 91L, "Raffaele", 0 },
                    { 92L, "Riccardo", 0 },
                    { 93L, "Salvatore", 0 },
                    { 95L, "Simone", 0 },
                    { 78L, "Leonardo", 0 },
                    { 114L, "Michele", 1 },
                    { 116L, "Silvestri", 1 },
                    { 136L, "Messina", 1 },
                    { 137L, "Portella", 1 },
                    { 138L, "Dalpiaz", 1 }
                });

            migrationBuilder.InsertData(
                table: "Name",
                columns: new[] { "Id", "Text", "Type" },
                values: new object[,]
                {
                    { 139L, "Vanacore", 1 },
                    { 140L, "Ciarrocchi", 1 },
                    { 141L, "Girolamo", 1 },
                    { 142L, "Granieri", 1 },
                    { 143L, "D'Aleo", 1 },
                    { 144L, "Talluto", 1 },
                    { 145L, "La Viola", 1 },
                    { 146L, "Colaizzi", 1 },
                    { 147L, "Frezza", 1 },
                    { 148L, "Barsotti", 1 },
                    { 149L, "Riccardo", 1 },
                    { 150L, "Dena", 1 },
                    { 135L, "Acocella", 1 },
                    { 115L, "Geronimo", 1 },
                    { 134L, "Mollo", 1 },
                    { 132L, "Lucido", 1 },
                    { 117L, "Falasca", 1 },
                    { 118L, "Blancato", 1 },
                    { 119L, "Raimondo", 1 },
                    { 120L, "Luzi", 1 },
                    { 121L, "Riviera", 1 },
                    { 122L, "Morreale", 1 },
                    { 123L, "Cozzi", 1 },
                    { 124L, "Pera", 1 },
                    { 125L, "Ditta", 1 },
                    { 126L, "Peduto", 1 },
                    { 127L, "Azzarello", 1 },
                    { 128L, "Maiorino", 1 },
                    { 129L, "Bonaccorsi", 1 },
                    { 130L, "Valentino", 1 },
                    { 131L, "Di Croce", 1 },
                    { 133L, "Satriano", 1 },
                    { 77L, "Jacopo", 0 },
                    { 76L, "Giuseppe", 0 },
                    { 75L, "Giulio", 0 },
                    { 21L, "Elisa", 0 },
                    { 22L, "Emily", 0 },
                    { 23L, "Emma", 0 },
                    { 24L, "Eva", 0 },
                    { 25L, "Francesca", 0 },
                    { 26L, "Gaia", 0 },
                    { 27L, "Giada", 0 }
                });

            migrationBuilder.InsertData(
                table: "Name",
                columns: new[] { "Id", "Text", "Type" },
                values: new object[,]
                {
                    { 28L, "Ginerva", 0 },
                    { 29L, "Gioia", 0 },
                    { 30L, "Giorgia", 0 },
                    { 31L, "Giulia", 0 },
                    { 32L, "Greta", 0 },
                    { 33L, "Irene", 0 },
                    { 34L, "Isabel", 0 },
                    { 35L, "Ludovica", 0 },
                    { 20L, "Eleonora", 0 },
                    { 36L, "Margherita", 0 },
                    { 19L, "Elena", 0 },
                    { 17L, "Chiara", 0 },
                    { 2L, "Alessia", 0 },
                    { 3L, "Alice", 0 },
                    { 4L, "Anita", 0 },
                    { 5L, "Anna", 0 },
                    { 6L, "Arianna", 0 },
                    { 7L, "Asia", 0 },
                    { 8L, "Aurora", 0 },
                    { 9L, "Azzurra", 0 },
                    { 10L, "Beatrice", 0 },
                    { 11L, "Benedetta", 0 },
                    { 12L, "Bianca", 0 },
                    { 13L, "Camilla", 0 },
                    { 14L, "Carlotta", 0 },
                    { 15L, "Caterina", 0 },
                    { 16L, "Cecilia", 0 },
                    { 18L, "Chloe", 0 },
                    { 37L, "Maria", 0 },
                    { 38L, "Marta", 0 },
                    { 39L, "Martina", 0 },
                    { 60L, "Diego", 0 },
                    { 61L, "Domenico", 0 },
                    { 62L, "Edoardo", 0 },
                    { 63L, "Elia", 0 },
                    { 64L, "Emanuele", 0 },
                    { 65L, "Enea", 0 },
                    { 66L, "Federico", 0 },
                    { 67L, "Filippo", 0 },
                    { 68L, "Francesco", 0 },
                    { 69L, "Franco", 0 },
                    { 70L, "Gabriel", 0 }
                });

            migrationBuilder.InsertData(
                table: "Name",
                columns: new[] { "Id", "Text", "Type" },
                values: new object[,]
                {
                    { 71L, "Giacomo", 0 },
                    { 72L, "Gioele", 0 },
                    { 73L, "Giorgio", 0 },
                    { 74L, "Giovanni", 0 },
                    { 59L, "Davide", 0 },
                    { 58L, "Daniel", 0 },
                    { 57L, "Christian", 0 },
                    { 56L, "Brando", 0 },
                    { 40L, "Matilde", 0 },
                    { 41L, "Melissa", 0 },
                    { 42L, "Mia", 0 },
                    { 43L, "Miriam", 0 },
                    { 44L, "Nicole", 0 },
                    { 45L, "Noemi", 0 },
                    { 46L, "Rebecca", 0 },
                    { 151L, "Gregorio", 1 },
                    { 47L, "Sara", 0 },
                    { 49L, "Viola", 0 },
                    { 50L, "Vittoria", 0 },
                    { 51L, "Abramo", 0 },
                    { 52L, "Alessandro", 0 },
                    { 53L, "Alessio", 0 },
                    { 54L, "Andrea", 0 },
                    { 55L, "Antonio", 0 },
                    { 48L, "Sofia", 0 },
                    { 152L, "Parrinello", 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Name");
        }
    }
}
