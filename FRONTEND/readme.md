**Hotovo:**

* Layout (navbar, main container, footer)
* Domovská stránka
* Seznam sportů (název, seznam sportovišť, prokliky na detail sportoviště)
* Seznam sportovišť (název, dostupné sporty, prokliky na detail)
* Detail sportoviště (areál, adresa, sporty, ceník, aktuální rezervace, proklik na vytvoření rezervace s předvyplněným sportovištěm)
* Rezervace (formulář s dynamickým výběrem sportu podle sportoviště + možnost předvyplnění přes query param)
* Login (JWT autentizace)
* Logout (mazání tokenu z localStorage)
* Registrace (vytvoření uživatele přes API)
* Profil uživatele (editace osobních údajů, zobrazení rezervací (včetně možnosti zrušení), zobrazení zapůjčeného vybavení, zobrazení support ticketů, zobrazení membershipu)





**Použité technologie:**

* Frontend: Blazor WebAssembly (.NET)
* Ukládání tokenu a uživatele na frontendu: localStorage (Blazored.LocalStorage)





**Technické poznámky:**

* BaseAddress pro API je nastaven v Program.cs
* API a frontend musí běžet na stejném protokolu (http/http), jinak problém





**!!! Změny co musí být provedeny v API !!!:**

* UsersController.cs > CreateUser > line 102: Membership = "STANDART" (předtím napsáno malým písmem a nefungovalo kvůli neshodě s ceníkem)

&#x09;



&#x09;

