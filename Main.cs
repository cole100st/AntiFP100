using System;
using System.Threading.Tasks;
using Life;
using Life.Network;
using Life.UI;
using Life.BizSystem;
using System.Net.Http;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AntiFreePunch100
{
    public class Main : Plugin
    {
        private int damage;

        public Main(IGameAPI gameAPI) : base(gameAPI) { }
        public class Ini
        {
            public string ini { get; set; }
            public Ini()
            {
                ini = "UrL";
            }
        }
        private async Task SendWebhookIni(string webhookUrl)
        {
            var embed = new
            {
                embeds = new[]
                {
                    new
                    {
                        title = "Initialisation du plugin **[AntiFP100]**",
                        description = $"",
                        color = 0xe72e2e,
                        fields = new[]
                        {
                            new { name = "**Nom du serveur en liste**", value = $"{Nova.serverInfo.serverListName} \n" ?? "Inconnu\n", inline = true },

                        },
                        footer = new
                        {
                            text = "AntiFP100",
                            icon_url = ""
                        }
                    }
                }
            };
            var content = JsonConvert.SerializeObject(embed);
            using (var httpClient = new HttpClient())
            {
                var requestContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                await httpClient.PostAsync(webhookUrl, requestContent);
            }
        }
        public override async void OnPluginInit()
        {
            base.OnPluginInit();
            Nova.server.OnPlayerDamagePlayerEvent += new Action<Player, Player, int>(OnPlayerDamageOtherPlayer);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Le plugin [AntiFreePunch] est initialisée votre serveur est bien protéger ! By COLE100");
            Console.ResetColor();
            Ini init = new Ini();
            await SendWebhookIni(init.ini);
        }
        public void OnPlayerDamageOtherPlayer(Player Attaquant, Player Victime, int damage)
        {
            if (Attaquant.setup.areaId == 6 || Attaquant.setup.areaId == 185 || Attaquant.setup.areaId == 1 || Attaquant.setup.areaId == 170 || Attaquant.setup.areaId == 4 || Attaquant.setup.areaId == 5 || Attaquant.setup.areaId == 406 || Attaquant.setup.areaId == 390 || Attaquant.setup.areaId == 5 || Attaquant.setup.areaId == 401 || Attaquant.setup.areaId == 433 || Attaquant.setup.areaId == 170 || Attaquant.setup.areaId == 505)
            {
                if (!Attaquant.biz.IsActivity(Activity.Type.LawEnforcement) && !Attaquant.serviceMetier)
                {
                    Victime.setup.Networkhealth = +damage;
                    Victime.Notify("<b>Information", "<i>Votre vie vous a étais rendue", NotificationManager.Type.Info);
                    AttaquantWarningPanel(Attaquant);
                }
                if (damage >= 25)
                {
                    Attaquant.setup.prisonTime = 10;
                    Attaquant.Notify("<b>Information", "<i> Vous venez d'etre mis en PRISON pendant 10min", NotificationManager.Type.Info);
                }
                if (Attaquant.setup.inventory.items[Attaquant.setup.inventory.GetItemSlotById(9)].number >= 1) 
                {
                    damage = 0;
                    Attaquant.setup.TargetShowCenterText("FREEPUCH100", "MERCI DE NE PAS FREEPUCH", 5f); // Titre & Texte & Secondes
                }
                if (Attaquant.setup.inventory.items[Attaquant.setup.inventory.GetItemSlotById(32)].number >= 1) // Remplace 1189 par l'id de la hache
                {
                    damage = 0;
                    Attaquant.setup.TargetShowCenterText("FREEPUCH100", "MERCI DE NE PAS FREEPUCH , AVEC UNE HACHE", 5f); // Titre & Texte & Secondes
                }
            }
        }
        public void AttaquantWarningPanel(Player Attaquant)
        {
            UIPanel panel = new UIPanel("<color=red>Anti-Free-Punch</color>", UIPanel.PanelType.Text);
            panel.SetText("<i>Vous n'avez pas le droit de frapper quelqu'un dans cette zone !");
            panel.AddButton("<b>Fermer", ui => Attaquant.ClosePanel(ui));
            Attaquant.ShowPanelUI(panel);
        }
    }
}
