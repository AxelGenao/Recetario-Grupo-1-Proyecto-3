using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetario
{
    internal class Recetario : RecetarioBaseVisitor<object>
    {
        int recipeId = 1;
        int elaborationId = 1;
        int ingredientId = 1;
        string elaborationInserts = "";
        string ingredientsInserts = "";
        List<string> output = new List<string>();

        public List<string> Output { get { return output; } }

        public override object VisitCalorias([NotNull] RecetarioParser.CaloriasContext context)
        {
            return context.NUMBER().GetText();
        }

        public override object VisitCoccion([NotNull] RecetarioParser.CoccionContext context)
        {
            string[] coccion = new string[] { context.NUMBER().GetText(), context.TEXT().GetText() };
            return coccion;
        }

        public override object VisitElaboracion([NotNull] RecetarioParser.ElaboracionContext context)
        {
            elaborationInserts = "";
            int i = 1;
            foreach (var item in context.ela_item())
            {
                elaborationInserts += $"INSERT INTO cooking_steps (id, recipe_id, step_number, description) VALUES ({elaborationId}, {recipeId}, {i}, '{Visit(item)}');\n";
                i++;
                elaborationId++;
            }
            return elaborationInserts;
        }

        public override object VisitEla_item([NotNull] RecetarioParser.Ela_itemContext context)
        {
            return context.TEXT().GetText();
        }

        public override object VisitIngredientes([NotNull] RecetarioParser.IngredientesContext context)
        {
            ingredientsInserts = "";
            foreach (var item in context.ing_item())
            {
                string[] ingItem = Visit(item) as string[];
                if (ingItem.Length == 3)
                {
                    ingredientsInserts += $"INSERT INTO ingredients (id, recipe_id, name, quantity, unit_id) VALUES ({ingredientId}, {recipeId}, '{ingItem[0]}', {ingItem[1]}, {ingItem[2]});\n";
                }
                else if (ingItem.Length == 2)
                {
                    ingredientsInserts += $"INSERT INTO ingredients (id, recipe_id, name, quantity, unit_id) VALUES ({ingredientId}, {recipeId}, '{ingItem[0]}', {ingItem[1]});\n";
                }
                ingredientId++;
            }
            return ingredientsInserts;
        }

        public override object VisitIng_item([NotNull] RecetarioParser.Ing_itemContext context)
        {
            string? unit = "";
            string[] ingItem;
            try
            {
                switch (context.unit.Type)
                {

                    case RecetarioParser.CUCHARADA:
                        unit = "1";
                        break;
                    case RecetarioParser.CUCHARADITA:
                        unit = "2";
                        break;
                    case RecetarioParser.TAZA:
                        unit = "3";
                        break;
                    default:
                        break;
                }
            }
            catch (NullReferenceException e)
            {
            }
            if (unit != "")
            {
                ingItem = new string[] { context.TEXT().GetText(), context.NUMBER().GetText(), unit};
            }
            else
            {
                ingItem = new string[] { context.TEXT().GetText(), context.NUMBER().GetText()};
            }
            return ingItem;
        }

        public override object VisitNombre([NotNull] RecetarioParser.NombreContext context)
        {
            string nombre = context.TEXT().GetText();
            output.Add($"\n-- Receta: {nombre}");
            return nombre;
        }

        public override object VisitPorciones([NotNull] RecetarioParser.PorcionesContext context)
        {
            return context.NUMBER().GetText();
        }

        public override object VisitPreparacion([NotNull] RecetarioParser.PreparacionContext context)
        {
            string[] preparacion = new string[]{context.NUMBER().GetText(), context.TEXT().GetText()};
            return preparacion;
        }

        public override object VisitReceta([NotNull] RecetarioParser.RecetaContext context)
        {
            string nombre = Visit(context.nombre()) as string;
            string porciones = Visit(context.porciones()) as string;
            string[]? preparacion, coccion;
            string? prepTime, prepUnit, coccionTime, coccionUnit;

            string insertNames = "INSERT INTO recipes (id, name, portions,";
            string insertValues = $"VALUES ({recipeId}, '{nombre}', {porciones},";

            try
            {
                preparacion = Visit(context.preparacion()) as string[];
                prepTime = preparacion[0];
                prepUnit = preparacion[1];
                insertNames += " prep_time, prep_time_unit,";
                insertValues += $" {prepTime}, '{prepUnit}',";
            }
            catch (NullReferenceException e)
            {

            }
            try
            {
                coccion = Visit(context.coccion()) as string[];
                coccionTime = coccion[0];
                coccionUnit = coccion[1];
                insertNames += " cook_time, cook_time_unit,";
                insertValues += $" {coccionTime}, '{coccionUnit}',";

            }
            catch (NullReferenceException e)
            {

            }
            string calorias = Visit(context.calorias()) as string;

            string recipes = $"{insertNames} calories) {insertValues} {calorias});\n";

            Visit(context.ingredientes());
            Visit(context.elaboracion());

            recipeId++;

            string inserts = recipes + ingredientsInserts + elaborationInserts;
            output.Add(inserts);
            return inserts;
        }

        public override object VisitRecetario([NotNull] RecetarioParser.RecetarioContext context)
        {
            return base.VisitRecetario(context);
        }
    }
}
