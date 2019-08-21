using System;
using System.Collections.Generic;
using Informer.Controls;
using Informer.Models;
using Informer.Utils;
using Xamarin.Forms;

namespace Informer.Utils
{
    public class ContactsGenerator
    {
        public static List<View> Generate(Member member)
        {
            List<View> result = new List<View>();

            GenerateSimple(member, result);
            GenerateCategories(member, result);
            GeneratePhones(member, result);
            GenerateEmails(member, result);
            GenerateSites(member, result);
            GenerateVKs(member, result);
            GenerateFBs(member, result);
            GenerateInsts(member, result);

            return result;
        }

        private static void GenerateSimple(Member member, List<View> contacts)
        {
            if (!String.IsNullOrEmpty(member.Stand))
            {
                contacts.Add(new Label { Text = "Стенд: " + member.Stand, Margin = new Thickness(0) });
            }
        }

        private static void GenerateCategories(Member member, List<View> contacts)
        {
            foreach (String category in member.Categories)
            {
                contacts.Add(new Label { Text = "Тематика: " + category, Margin = new Thickness(0) });
            }
        }

        private static void GeneratePhones(Member member, List<View> contacts)
        {
            foreach (String phone in member.Phones) 
            {
                contacts.Add(GenerateLink("Телефон", "tel:"+phone, phone, new Thickness(0)));
            }
        }

        private static void GenerateEmails(Member member, List<View> contacts)
        {
            foreach (String email in member.Emails)
            {
                contacts.Add(GenerateLink("Почта", "mailto:" + email, email, new Thickness(0)));
            }
        }

        private static void GenerateSites(Member member, List<View> contacts)
        {
            foreach (String site in member.Sites)
            {
                contacts.Add(GenerateLink("Сайт", site, site, new Thickness(0)));
            }
        }

        private static void GenerateVKs(Member member, List<View> contacts)
        {
            foreach (String vk in member.VKs)
            {
                contacts.Add(GenerateLink("ВКонтакте", vk, vk, new Thickness(0)));
            }
        }

        private static void GenerateFBs(Member member, List<View> contacts)
        {
            foreach (String fb in member.FBs)
            {
                contacts.Add(GenerateLink("Фейстбук", fb, fb, new Thickness(0)));
            }
        }

        private static void GenerateInsts(Member member, List<View> contacts)
        {
            foreach (String inst in member.Insts)
            {
                contacts.Add(GenerateLink("Инстаграм", inst, inst, new Thickness(0)));
            }
        }

        private static HTMLLabel GenerateLink(String caption, String link, String value, Thickness margin)
        {
            return new HTMLLabel
            {
                Margin = margin,
                Text = caption + ": " + "<a href=\"" + link + "\">" + value + "</a>"
            };
        }

    }
}
