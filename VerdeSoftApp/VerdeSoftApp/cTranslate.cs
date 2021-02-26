using System;
using System.Collections.Generic;
using System.Text;

namespace VerdeSoftApp
{
    public static class cTranslate
    {
        private static List<eTranslate> translations = null;
        private static void initTranslations()
        {
            translations = new List<eTranslate>();
            translations.Add(new eTranslate() { Code = "Kassa", NL = "Kassa", FR = "Caisse", EN = "Cashregister", DE = "" });
            translations.Add(new eTranslate() { Code = "Klanten", NL = "Klanten", FR = "Clients", EN = "Customers", DE = "" });
            translations.Add(new eTranslate() { Code = "Betalen", NL = "Betalen", FR = "Payer", EN = "Pay", DE = "" });
            translations.Add(new eTranslate() { Code = "Terug", NL = "Terug", FR = "Retour", EN = "Return", DE = "" });
            translations.Add(new eTranslate() { Code = "Lijst", NL = "Lijst", FR = "Liste", EN = "List", DE = "" });
            translations.Add(new eTranslate() { Code = "Versturen", NL = "Versturen", FR = "Expédier", EN = "Send", DE = "" });
            translations.Add(new eTranslate() { Code = "Etiketten", NL = "Etiketten", FR = "Etiquettes", EN = "Labels", DE = "" });
            translations.Add(new eTranslate() { Code = "Bestellingen", NL = "Bestellingen", FR = "Commandes", EN = "Orders", DE = "" });
            translations.Add(new eTranslate() { Code = "Inventaris", NL = "Inventaris", FR = "Inventaire", EN = "Inventory", DE = "" });

            translations.Add(new eTranslate() { Code = "Cash", NL = "Cash", FR = "Cash", EN = "InventCashory", DE = "" });
            translations.Add(new eTranslate() { Code = "Bancontact", NL = "Bancontact", FR = "Bancontact", EN = "Bancontact", DE = "" });
            translations.Add(new eTranslate() { Code = "Waardebon", NL = "Waardebon", FR = "Bon de valeur", EN = "Voucher", DE = "" });
            translations.Add(new eTranslate() { Code = "Cheque", NL = "Cheque", FR = "Cheque", EN = "Cheque", DE = "" });

            translations.Add(new eTranslate() { Code = "Totaal", NL = "Totaal", FR = "Total", EN = "Total", DE = "" });
            translations.Add(new eTranslate() { Code = "Betaald", NL = "Betaald", FR = "Payé", EN = "Payed", DE = "" });
            translations.Add(new eTranslate() { Code = "Betaal", NL = "Betaal", FR = "A payé", EN = "To pay", DE = "" });

            translations.Add(new eTranslate() { Code = "Synchroniseer", NL = "Synchroniseer", FR = "Synchroniser", EN = "Sync", DE = "" });
            translations.Add(new eTranslate() { Code = "Instellingen", NL = "Instellingen", FR = "Paramètres", EN = "Params", DE = "" });
            translations.Add(new eTranslate() { Code = "Afsluiten", NL = "Afsluiten", FR = "Quiter", EN = "Close", DE = "" });

            translations.Add(new eTranslate() { Code = "Code", NL = "Code", FR = "Code", EN = "Code", DE = "" });
            translations.Add(new eTranslate() { Code = "Naam", NL = "Naam", FR = "Nom", EN = "Name", DE = "" });
            translations.Add(new eTranslate() { Code = "Barcode", NL = "Barcode", FR = "Code barre", EN = "Barcode", DE = "" });

            translations.Add(new eTranslate() { Code = "Verwijderen", NL = "Verwijderen", FR = "Supprimer", EN = "Delete", DE = "" });
            translations.Add(new eTranslate() { Code = "Wijzigen", NL = "Wijzigen", FR = "Modifier", EN = "Modify", DE = "" });
            translations.Add(new eTranslate() { Code = "Zoeken", NL = "Zoeken", FR = "Chercher", EN = "Search", DE = "" });
            translations.Add(new eTranslate() { Code = "Kiezen", NL = "Kiezen", FR = "Choisir", EN = "Choose", DE = "" });
            translations.Add(new eTranslate() { Code = "Resultaat", NL = "Resultaat", FR = "Résultat", EN = "Result", DE = "" });

            translations.Add(new eTranslate() { Code = "Aantal", NL = "Aantal", FR = "Qté", EN = "Qty", DE = "" });
            translations.Add(new eTranslate() { Code = "Prijs", NL = "Prijs", FR = "Prix", EN = "Price", DE = "" });
            translations.Add(new eTranslate() { Code = "Korting", NL = "Korting", FR = "Réduction", EN = "Discount", DE = "" });

            translations.Add(new eTranslate() { Code = "Artikels", NL = "Artikels", FR = "Articles", EN = "Articles", DE = "" });
            translations.Add(new eTranslate() { Code = "Gebruikers", NL = "Gebruikers", FR = "Utilisateurs", EN = "Users", DE = "" });
            translations.Add(new eTranslate() { Code = "Initialiseer", NL = "Initialiseer", FR = "Initialiser", EN = "Init", DE = "" });

            translations.Add(new eTranslate() { Code = "Vraag", NL = "Vraag", FR = "Question", EN = "Question", DE = "" });
            translations.Add(new eTranslate() { Code = "Ja", NL = "Ja", FR = "Oui", EN = "Yes", DE = "" });
            translations.Add(new eTranslate() { Code = "Nee", NL = "Nee", FR = "Non", EN = "No", DE = "" });
            translations.Add(new eTranslate() { Code = "Alles verwijderen", NL = "Alles verwijderen", FR = "Supprimer tout", EN = "Delete all", DE = "" });

            translations.Add(new eTranslate()
            {
                Code = "Geen artikels gevonden",
                NL = "Geen artikels gevonden",
                FR = "Aucun article trouvé",
                EN = "Article not found",
                DE = ""
            });

            translations.Add(new eTranslate() { Code = "Identificatie", NL = "Identificatie", FR = "Identification", EN = "Identification", DE = "" });
            translations.Add(new eTranslate() { Code = "Ticket", NL = "Ticket", FR = "Ticket", EN = "Ticket", DE = "" });
            translations.Add(new eTranslate() { Code = "Taal", NL = "Taal", FR = "Langue", EN = "Language", DE = "" });
            translations.Add(new eTranslate() { Code = "Stoppen prijs", NL = "Stoppen prijs", FR = "Arrêter prix", EN = "Hold on price", DE = "" });
            translations.Add(new eTranslate() { Code = "Betaalscherm", NL = "Betaalscherm", FR = "Ecran paiement", EN = "Payment screen", DE = "" });
            translations.Add(new eTranslate() { Code = "Klantcode", NL = "Klantcode", FR = "Code client", EN = "Client code", DE = "" });
            translations.Add(new eTranslate() { Code = "Opslaan", NL = "Opslaan", FR = "Enregistrer", EN = "Save", DE = "" });
            translations.Add(new eTranslate() { Code = "Lijst verstuurd", NL = "Lijst verstuurd", FR = "Liste expédiée", EN = "List send", DE = "" });
            translations.Add(new eTranslate() { Code = "Bericht", NL = "Bericht", FR = "Message", EN = "Message", DE = "" });

            translations.Add(new eTranslate() { Code = "Breuk", NL = "Breuk", FR = "Dommage", EN = "Damage", DE = "" });
            translations.Add(new eTranslate() { Code = "Controle", NL = "Controle", FR = "Vérifier", EN = "Control", DE = "" });

            translations.Add(new eTranslate() { Code = "Stock", NL = "Stock", FR = "Stock", EN = "Stock", DE = "" });
            translations.Add(new eTranslate() { Code = "Plaats", NL = "Plaats", FR = "Endroit", EN = "Place", DE = "" });
            translations.Add(new eTranslate() { Code = "Opmerking", NL = "Opmerking", FR = "Remarque", EN = "Remark", DE = "" });
            translations.Add(new eTranslate() { Code = "Mutaties", NL = "Mutaties", FR = "Mutations", EN = "Mutations", DE = "" });
            translations.Add(new eTranslate() { Code = "Aant", NL = "Aant", FR = "Qté", EN = "Qty", DE = "" });
            translations.Add(new eTranslate() { Code = "Verwerkt", NL = "Verwerkt", FR = "Traité", EN = "Processed", DE = "" });

            translations.Add(new eTranslate() { Code = "Prijzen aanpassen", NL = "Prijzen aanpassen", FR = "Modifier les prix", EN = "Modify prices", DE = "" });

            translations.Add(new eTranslate() { Code = "Niet gevonden", NL = "Niet gevonden", FR = "Pas trouvé", EN = "Not found", DE = "" });
            translations.Add(new eTranslate() { Code = "Doorgaan", NL = "Doorgaan", FR = "Continuer", EN = "Continue", DE = "" });

            translations.Add(new eTranslate() { Code = "Opsplitsen", NL = "Opsplitsen", FR = "Séparer", EN = "Split", DE = "" });
            translations.Add(new eTranslate() { Code = "Lengte barcode", NL = "Lengte barcode", FR = "Longueur code barres", EN = "Length barcode", DE = "" });

            translations.Add(new eTranslate() { Code = "Herhaal aantal", NL = "Herhaal aantal", FR = "Répéter qté", EN = "Repeat quantity", DE = "" }); //1.0.4 b

            translations.Add(new eTranslate() { Code = "Vervolg", NL = "Vervolg", FR = "Continuation", EN = "Next", DE = "" });
            translations.Add(new eTranslate() { Code = "Opslaan gelukt", NL = "Opslaan gelukt", FR = "Enregistrement réussi", EN = "Save successful", DE = "" });

            translations.Add(new eTranslate() { Code = "Keyboard Alpha", NL = "Keyboard Alphanumeric", FR = "Clavier alphanumérique", EN = "Keyboard Alphanumeric", DE = "" });
            translations.Add(new eTranslate() { Code = "Zoekveld", NL = "Zoekveld", FR = "Champ de recherche", EN = "Searchfield", DE = "" });
            translations.Add(new eTranslate() { Code = "NietsSearchField", NL = "Niets", FR = "Ne rien", EN = "Nothing", DE = "" });
        }

        public static string getValue(string Code)
        {
            if (translations == null) initTranslations();
            
            foreach(eTranslate e in translations)
            {
                if (e.Code == Code)
                {
                    if (cData.settings != null)
                    {
                        if (cData.settings.Language == "") return e.NL;
                        if (cData.settings.Language == "N") return e.NL;
                        if (cData.settings.Language == "F") return e.FR;
                        if (cData.settings.Language == "E") return e.EN;
                        if (cData.settings.Language == "D") return e.DE;
                    }
                }
            }
            return Code;
        }
    }
}
