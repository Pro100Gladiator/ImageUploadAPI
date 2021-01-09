using ImageUploadAPI.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageUploadAPI.Uploaders;
using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace ImageUploadAPI.Uploaders.Tests
{
    [TestClass()]
    public class UploadersTests
    {
        [TestMethod()]
        public void UrlProcessTest()
        {
            //arrange
            var client = new RestClient("http://localhost:5000/api/ImageUpload");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("urls", "https://fainaidea.com/wp-content/uploads/2019/06/acastro_190322_1777_apple_streaming_0003.0.jpg");
            request.AddParameter("urls", "https://avatarko.ru/img/kartinka/1/avatarko_anonim.jpg");

            //act
            IRestResponse response = client.Execute(request);

            //assert
            //Assert.AreEqual(@"Picture 1 successfully downloaded.\nPicture 2 successfully downloaded.\nTotal pictures uploaded: 2 from 2", response.Content);
            Assert.IsTrue(System.IO.File.Exists(@"W:\Upload\avatarko_anonim.jpg"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Upload\acastro_190322_1777_apple_streaming_0003.0.jpg"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Preview\preview_avatarko_anonim.jpg"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Preview\preview_acastro_190322_1777_apple_streaming_0003.0.jpg"));

            //after test
            System.IO.Directory.Delete(@"W:\Upload", true);
            System.IO.Directory.Delete(@"W:\Preview", true);

        }

        [TestMethod()]
        public void JsonProcessTest()
        {
            //arrange
            var client = new RestClient("http://localhost:5000/api/ImageUpload");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "[\r\n\t{\r\n\t\t\"base64\":\"url(" +
                "'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAwIAAAD6CAMAAAAlSwBjAAADAFBMVEVMaXEATJf" +
                "///8ATJcATJcATJcATJcATJf///8ATJcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJ" +
                "cATJf49/oATJcATJcATJcATJcATJf////v7PIATJcATJcATJcATJcATJf39vkATJcATJcATJcATJf///8ATJfz8" +
                "PcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJcATJf8+/0ATJcATJcATJcATJcATJcATJcATJcATJcA" +
                "TJcATJcATJfk3+wATJcATJcATJcATJcATJcATJfq5+wATJfk3+wATJfs6e/z8PcATJcATJcATJfb1eLk3+wATJc" +
                "ATJfY0eQATJcATJcATJfa0uQATJfPx9oATJfZ0eQATJcATJfNw9sATJfPxtvNw9vDt9QATJcATJfNw9sATJcATJ" +
                "cATJe1pssATJcATJe4q80ATJcATJevnce4q80ATJe3q80ATJeeh7yjkL4ATJd9mcMATJcATJcATJcATJcATJcAT" +
                "JeLb6+lk8AATJeXfbebhbkATJeLca4ATJd/X6cATJd7WaWKbK8ATJcATJcATJd/X6d6WaV1UaCKbK+ZgbgATJd9" +
                "XKaKbK8ATJduSJ1kN5YATJcATJd1UaBqP5sATJeXfbdxhriLca6KbK8ATJcATJcATJduRp1+gbVuRp0ATJdlNph" +
                "1UaBdLJN/X6d1UaAATJdqP5uDcK4ATJcATJdrQZxcLZFuSpx/X6cATJcATJd4VaNsQpwATJdTHIwATJdlNphAAI" +
                "IATJdWII4ATJdqP5sATJduSJ17WaUATJcATJcATJdkN5ZuRp1mb61zdrBxca8ATJdcLZFsQpz///8ATJdkN5ZoP" +
                "JpTHIxWII5lNpj6+/fZ0eRqP5tMA4lYJo9fLJVPDYrDt9Tz8Pfk3+yXfbdAAIKbhbmeh7z49/rq5+yqmMSLca64" +
                "q82jkL5uRp3Nw9u/sdB1UaB7WaWyoMnPx9rb1eKKbK9/X6duSpxRIo5jf7S5VfWjAAAA1nRSTlMAAQECAwQFBgY" +
                "HCAkKCwwNDg8QERITFBUWFhcYGRobGxscHR4fICAhIiMkJCUlJicoKSorLC0uLzAxMTIzNDU2Nzg5Ojs8PD0+P0" +
                "BCRUZISEpKS05SU1NTVllZWlxdX2BiY2NmamptbW9vcnd3en2AgoSGiImMjY6PkpSVlZmZnJ+goqOmp6ipq6usr" +
                "a+ys7O0tri7vLy8vL2/wsLDw8bHys3Nzs/P0NHS1djZ2dvc3N3d3t/g4uLj5ufn5+fp7Ozs7/Dx8fLz8/X19vb3" +
                "+Pn7/Pz8/f3+LbG4NQAAQmxJREFUaN7sW3uQG/V916IXepxOzztZL3J6oAdCz5V2l31qtYqkM88CAVxwgQRwIIA" +
                "ZikuddpJO69RDaNxgSgPFQPFMgtupDc1k4pJxJhmKccjYzj8+WfjswSeUBJLMpJkmf5DpTAXGu6s7rXySdiUt3s" +
                "8fHvv2d97f/r7fz/f9U6kUKFCgQIECBQoUKFCgQIECBQoUKFCgQIECBQoUKFAgGtQGs80VMikHoeDC0AXn7GaD5" +
                "lP0RVqz3RPLo5XFulsRr4ILw1au1zAw7rObdZ8G9Z9x+NNIdSON5hMhn1kRr4ILw+gOxbNIabGGZAJOi6xpYHAE" +
                "Ukh1kSzEA/NWnSJaBQPYztk5fwwk6nUss+CSaQA9MxfMlxepQtzvMCoSVTCUO7D7oiCxsVIMu2cBuem/OwpX62j" +
                "S71AyYAWjhRJ2XxzuOIO4d1Y+m9Y4Q1ClCse9FrUiQQUiwOyOFKpVJDIvj2jaEkhTi1hsbgZQRKdANJhcYbhOZ0" +
                "PWad8oMJ+AauWUR7H/CiSIruNUDUl6prlhYOg4gBoasmkVcSmQJMS2BqBqKbswrQmmKQSWq9k5pfavQNKAKMkwx" +
                "ahlCrdmjRUr5ZhdrwhJgbTQ2iJUFYo7po0AcaRCBSwaRUAKpId6xotV0eQ0kcCaQBgioHQAFIwvHvJiFTRtm5Yk" +
                "OIqWiIBRqYEqGCeMXqyMxqch89QswCQeMCgEUDB22+vFSmhk4t2y+SJGRE0KARRMAIAhhBOwf6LaZ8ugRNqstME" +
                "UTCoxNiRIPO+aGAn0IRTPOxQCKJgkCWazBB6fTClG7SzisF8pgyqYNAncEIG4J6CI+hiOpmYUASiYgrw4gRIp85" +
                "ijIY0DIorzyuErmA448jjqHutomj5GonFZ3QUDdEJGQq27mOtZOrXggckqydOGYDI9viYBYIOooltertKfcwk8c" +
                "ue9F+1gk86VDwhoujUfllet25knUNeYMgJdgCISspqG0HuKJcgr8DCA0OCc7mIkgMaRKWERAQrMFWnULytHrwuj" +
                "VGQs1syYYRCfrOyDLVdGE1bhODKJldOzFx8DTPEykZkTfDwThZmiS1aCni/SeemnqAEHwuStsjIOXoLK9h8qdOV" +
                "KhPsiK+8CTpQpevsumU0SpaBBTh9lztD4vMQ5jCZAl+KyihqMSQZZuJB660IYEzNcTAzQhRgyesE4x1NgsrK6Aq" +
                "UOE+WwpPppSFQIn6xEbQSZvH09yVSxkr2IOKBNVtH11LQtaRq2yOrL3EglI2GiagbrRbusDsRSqGTWp9mmfDV/0" +
                "dz5NGRq0Po0WxMrI/IS+Wy+CkmW2dnQmrzcosoEM4n1ukVDqlq4SPyANruYW68g1aESLi8/oE9WCZdELgavyisN" +
                "+MiyRwdIEJO17EXRIdAm6/kBOqmBMiQvDqgjDOWVIikOVGi/vPqoukwtO8hJaMF6QvvpZ4A6UocHKvjHGdAor0/" +
                "0kLWQ6BzQRJmS3GaCgtVCl1U3hzyrVvgWumRrhGv+se3uM5/d9KVtX/vWt7/7o5/8bOlnP/nRd7/9ra/95Zc2ff" +
                "YzUr94vop3jTfqvMFVK1zRrmBak2HiMhO9g6gmRI5YNPEaZpPZMdgogi9Jta+wWOg2DRq0nvcAXb9CjqNHdsUtW" +
                "3d+56et9kp7+XSrtXQOrdbp5c5PWj/9zs6tt1wh3cuNGN3VD3NlGHJV/Je5GlnQd6VUJbfMhD8LLWZE5QCQqCBy" +
                "G4w2gjW+3CzpMh22dlNA7UiUSwl+Yuiv5yROBy7ftG3PmyttTvVXo0OF9sqbe/5i0+WSvF+TqIf5uWOUrKTsq5T" +
                "FGi5V8nyL5yhDshN/sSYmBwwZ+TFAFayneArv6tT9e7gxjQNkQDvf59el7Htc9cWdP2i3hZS/iwjt9g92fvEq8b" +
                "cwVyvwNGM2w8DzPfIfa6qK8VvHkUpMJT8OgKKl8UawkpVdydyMUFae4NFSqLd5N0ZpmDc+4aAhqVK/K+/5+pvt1" +
                "nr0/7w7aL/59XuuFLlEAJZ5vtGSZwSuPen8JZJnDEw4apMdB1JVsQy3ESxn5FcsDNaCXJTvQiihoWCVJkQXHfxy" +
                "iSQZ8SU3fPnVleWlQbG88uqXb7hE1Fw4yU2LzObLCUHJukn+IICnGpedCmgTjDitbSNIy5ABFpRX9rBA1IJwPVc" +
                "dLoGck7MSsPj99Q1371pZbi0Ng9byyq67N4i1ET1Icl0jfaZv49BN4E4uFoYxu/w4EC+LwQFNtiRDBqj8i2FW6b" +
                "U5sm+VWBOl0uxzILYodvXjsi3PtU8vDY/T7ee2XCbOVuxV7ktVsXKqb8LowTlrAPhqEZUcOQCNrLyaOJmW4X0SY" +
                "57iYtcQnuk/KqrP41z04xSZ8hu2vNBuLY2GVvuFLWJ4Am2C4pLceQLpn+EBMSLB2hEzLrui0MccKI3a8dfECVkO" +
                "zjiZlIYLg9BuUQduu/feW7tM/SxRYO2dNkOLmPlduvnpkQlwjgRPb7509BoByfV59dly1ySN/cZ7v3BHd91HV8D" +
                "Z6EcdK3tlqAiaLDWiRYvjRYsMP1wVqnON4ATRVQi//cWDh48ePXxw981cMVAdI7k1ARFbxNftEIUA50iw47qRk2" +
                "GG03F/iVc3B65+4vtvHD165NDez/Pl7cdyrBtwyDAhPhcPZEeZeglg8mSALkNaWQsPoTw/Ftt9pPn2Nx57bPex5" +
                "pEnfDzzWGQ9hYNMijQpdMXW15eXxMPy61tH6xprIqSHcwIM5+ys2w83Tzz/+GNP/rh59CWEVykAMbZYZsZAWQ7S" +
                "GnJkdPgbgT4Utsnxq1UWnGvyBrAwlwHmXmoe/LzDrNfPzN3/w+buEBfw4V6uBIaK0wXZtGe5tSQmWst7No2kDQU" +
                "unncRnBOw/e2JI48ELXq9yX7b/uZ+hq8B7KitNkU4ZKkM5gIx9MycA0bnxNuJeoxjpnM1bkg6R3KOLPJs86Xzw6" +
                "MaYl/zSTYjsNFpLnJixCD+5Y++dXpJbJx+69ER5ibMNDc5G62xU4+m7c2D13/i+IDgU81XihxpEJj1iAuVMSYDg" +
                "IiDnlYIH7LIZ4Ewn0hqC2hMcxn3+A7Qu8iG8zNFbiBA/Uhzb4pbhe1v3guwsuYSxWBdhJnY63a1l6RAe9fwGYGt" +
                "kmBDRZ5luOnED6/nGf6nml9l1R5IcXfGXIvh8UnQkZk3a8WiwRyMDuXAjCARFoUBgG7GnyUIIqQZ2wFGyqz7cqM" +
                "x9r3pg2/fyF9217FXzsfG2iTG1kc85eDIO7jvwPKSNFg+cN+wm/KUFlg2oGyoqHvq+EP8VcyPj3KhUABlf8VaSo" +
                "7Nkat9GEHmF6w6cd7ow+AhJoDVKUqUhoDW5AFLBAzG58aXTAFximV9iHCzx3hr86nuhXub54UN+EoB1mhQo3L/s" +
                "m3tJenQ3jZko8zPjf3M0+w3ho8f6r5n+NfN+9m/21HWcViIjHpsItQ7o3kYL0F+sxhKCETwISaATcmCCB0igz1V" +
                "omAw5hzrfTMDCLNpX4LrkRm2Nx/uXrj9xIPnT9jJsN3PWWTEiemrdkjJgA4Hdgw1QqpJcPGAv8oG9rcf+0b3urt" +
                "OPMEGSSY0x4YFBXi8zTF7JA9TdMZhHF15gExmiNKmZmT6ASYvVMHAqGPc9y0NIMTWdDI0631cTx2/cZWwj371vF" +
                "TNVc7cwaN1FK95ZnlJWiw/c81QFOAC+9BGlg0PH3moe135yCts1A+gbG5syENjnxfusACrYAHzyO5HN4kbsYB5A" +
                "WOQlGcC7+6mAJvmunYfvbZ74Z++zVJgRjQKfO5lqRnQ4cDLnxuVAk6OAg90r8Pf2MdSQD1ZCnTUyJ2CGDIyo1bJ" +
                "Dh8RAIpbJ/JuAS9gfeLEXd0LHzi+3Si2F9j8WmtJerQObBbLCzxwdHv3uuuPv+iZEi9wTh7RIkNFLIDMCBBAmWJ" +
                "4dkJv50etaZqVG3B/8yvdC7/ZvA1gC4Zs/38Wzg2fut95YBwM6HDgtTsHpkASYfU+uMjWzMrNfd3rHmo+ztpcPV" +
                "rgSoTwpAblTEGwTIblNKig9yJVJDS5wUJtmmDNXZQ3IF9u7u+qfYQOHWP13ltiK6FOIjF09Hbn986+szQeDrw+K" +
                "AfUYYK17h6GLXaaDh2rdJmvF5u3qrjiKdsynMHyk5uQMASKVdxnlAsDHJkKEZud5A6iJVbV/Th3X8z+bLPL5/99" +
                "8+/OH6o6RLIlEjc9dF9g8/dONRrj4sBrg8ZCXortGDrJBNsueaT5PN9cPdjczw1PzXETErNkaqKBRRipgfOaqdB" +
                "wi/ECW6UrKedkt7hQd7NqD/MuC1x7/ARX8u6I/jDBVgzyEEtaX8035Htv+fcOA8bHgQMD5sS8wq+pyF2HCR9sfo" +
                "Wz77cfbv4Zz5ig7EnaqrHJhuPWOM3E+5tW3VgsrwmC+vlDV7EG+yfNVddGdgBOW8S53Fb9QLP58CfTD8HHmk0uO" +
                "zaW8qyziC0OeUfwmn/9mAHj48DLg9VGTUzmvBIDqSpXqrj2jebffFL+t9x/rPk4J191AWOZ4t3on7BYATdYQT19" +
                "lEuTQcaQMJjAOihcLzFGy+Xo5NMWO51iFTpBuQC+l28++8C1+fyND77UPPEF7nA9NDcSmaWGsyVXPf1OozFWDiw" +
                "/M1CPzIRyogtUedepbzrU3P/IzcX81fd/s9l8nNcQmsXYzhgQo+cnLlhTkCwnhPVLk6pAFukZwMSFe2aOwiI8+X" +
                "PqMBGCTFw0m+c/uuP5ZrO5b1/nj9038c4O4hqnFni4wfjLdp5pNMbNgR2DzEro0qid+0qCZ8kWn+wcyP59x5vNv" +
                "ffyfyPB3SjVg8g0lGQcYA0Wnl/WhEqQxLGQKV9PCz/1kZXUVFwwBaI19pi0Bapr9jlwx593CLD3kdv4B+kq51j/" +
                "6q2Fhop5t73XaIydA+1tgxyLt8IGiECizp8Dttz4cMc27H/8rlhXXodibOJnraSmoj1ljNKloHAwFK1Ke8vLnK8" +
                "Jd42Mcabkn46MXTW/GGXl68Pzq55ufOP7RHe5EMLmOd2oDTVZe18XA8bHgUHmRmfKWVZANozodnapvUdvXrU+yd" +
                "0nBQL1hemQLeDB+1habaJalNAPaNP1nGAUNJurQXbVlMAMIWxHTANy5fBP8t2D/9b9Ez/FDUFa0OIw5efr/uNsY" +
                "xIcaB0Y4P6ANstdhFTFmWiXt5t59jCxKqVCEZYlhgJqnRbpWnPVgqCqaRI16foXmmg1L+gDnEglPT2dCyC66ObC" +
                "R3xVFFs4tL/L39twjDMcvo3DxEGX/+OZRmMiHFjeNcA9Mk8twgW1cLkrqHY/f6TetdgAljhLYa+lp2dCQR+rYG5" +
                "BNU3WknqJXuyvcrXkNWdLViI61fRgrpzhjiFEgqY+FLBARICTe54a5trko2sZMC4OtB8dIBIiityczxyB2ftQQJ" +
                "eiEmouvij7pki8moUyFRAkSKESBiTSKkxQOXw06Z+qWT5NhkuIO7UyKmcSpMBMgYpze/fWEkN8yKb/PtuYGAfe2" +
                "rR+7xiphrh/BEuIXZACulSJVxmzMnn9NMkX8BDCd/tmYGn4aiVLQvVOdbhEzk/ZIJ+7zIsI9WkqNyNAAStI8UaC" +
                "TAXaNfjLrvinU43GxDhwes8VA0gRtvBKiCTiFKCAocMAMy8LpAPTJV+Vg6AFy/MOkpJgPsEM0UFAKAmnYdu0jbK" +
                "qU/wxB0OaKsz3pIAXIvlDcQv1+BBOYOtSozFBDixvXb/1DNWiPF8ZJpEFdS8KOEE+A1RzTE47ZQJWWYpMWijv9Z" +
                "bET951SSYpoBu6BN2/FKuNTCKMtGMErzimC1No0rqGAuYkRgR5wrWTyBBHd91/nW1MkgOt19dfFTJBFM+Zazw4n" +
                "rGuoYApgdAJXnHDhGBzExDhfLxv8GXKMxmDUDmkkhW7LORlhGbodUm60K8fZohApcwEMmUgWOafgtoFEUjSvJoC" +
                "cNHBY7YxV/YP7s4u3XGm0ZgoB5Z3XLp+veoyWMBsFretooAxjpCoV8N386X4BLx857VwvN8tHVOunBGoQmpT1aC" +
                "4u7FRqEBwpQ7QYB8G6KIwTiRnJhEmaTNMiO+49AHsk8iXowBg5dsZTaSSGqK3t3mp0ZgwB9rrn5sGIpUk3yJpbO" +
                "pVFJiFyIiBJzHAR4GTuCkAmKI4jib6lNqNOSYiIDArQovquIwgJWAd1QGqINyM0/hgHEuYJ1QrshZpT/eRurS9i" +
                "qLscz9TGGK+Y8M/n2pMmgOtpzesX5i53jVDlgJqp0XdnXhCExp+VxujGIEuaPvEQuWwAAfm+Yn/6HRcKAvNh7hJ" +
                "4aElwJonOgSYXKI8h/YuVPWmAOAVdHV9seW9RmPiHGhvWf+GZ3tXNta0xs4zAEUn1xIAjDGULDoFjagdogOAQDp" +
                "QjYo3rmMrCY1duHBEkAG6IIbn7aMQABjVffgwwgOskwIdd495hnjHZXtONSbPgdYLA4yMOtFSSL1eCjgRPDSqMR" +
                "9JjpYMTsQE82I7XPIKZsuiTS3r86SAGbAguGC5GPCR0Gh3LAB3bNR2jB9DDeujgMZPocMwQLVlqdGYAg4M4gZUL" +
                "oReGz/0poAmPzID9LHAaLZsvkiHBQ26GycFxhpdJGQSiQJ+Oq0R4AYR6RNzJkeMxfSpysj9hoV1egFDjEaGYsCG" +
                "f+npBE699zEE7tC0Vzo4zavorPRCi10qiBbnBp7bMMCunTCTMq2LAoBzVAZYwEp2RFU0JfsMf/pJpHfZCIhVIuJ" +
                "E4WYCtvb2bzEiJelw9EyqRHgkyKXXUACwgXRhuJTv7p5O4NT7736EX/5qqRcH2v/Tefbh+ywHln/34bs98MeOgr" +
                "d//24ffPh/HAfadw9UKsjR8KoIWygXGBEurJS1SpovRISK7jOFsiizy+o4KWDq/RRoUkmLAEmHtJJTwOgnyZR5q" +
                "P/qkn94p2fg8+uT5/CHUz048MGHHz363TJHiZO98H5Hvz/45cl++ANHgdO7LhnIxcaJUtgESE0BtY+iwhJXBHXZ" +
                "ssBYnJ/OiKE9VkrgrrAFQ6S/HzCHdFexpaCAJs9AC0N6zD9pNfpS4OdLPfKBlXcloMDSyg2Dbd1bYLoSJSkooA0" +
                "zYygnWYul3i5ck66IkBFrcnjvjFsL4kGV9LAUKjmjxBSIJof21H91pj8FTv76zFoOfCAJBZa/PGiEm+iaq5eAAv" +
                "oEAznGoCRuHDUJVCzh0Zt6TirX248FycxYbknOZKugUVIKqIb/jiv/8+wFKPCL986u4UBvL/CbX3TjN79tfbSU/" +
                "5M1y/gUaL165cBhirSBkD7NgLZx6IgqRiV6u/FE1TtqRqwGBa7nWFHEMpav6xSGqiLnHALd4SFwT6txAQqc/N8z" +
                "a2qjvSnw+w9W4eN0eYX3g9+uWdbi15lW7hnNlIpNgQ4Dcubx6IihQPXuT9lRdNTCuoMC1b3jI3JsLUNDqlrQTSc" +
                "Fdp7pR4GPNf3kH0+t5kDvQOjnyxcahvvV/5Nv/sFNXHcC91aydZYsWbZsy7FsN5KlSFZU/f61kVa/FZsfw+UOcg" +
                "ckHNNCmRRKaDPJJOWOMTRpSSiXG9L7o+TIwQzXKzAd5qaXablpoZcycX1Oje96Zylr2RLCMpgSOw52sDFk5lYrG" +
                "Sz81tbqrRaZvCGG7Gqf9d5+P+/7m/zYMOX9H5UUAhx9yCxkS0bq3ShY1HVhGbQSaKII1ZjYq/ypNIR13FJE4Knf" +
                "RZdC4OrlB02hDANXioNA5MOnSgmBtoCFNQIIi8cHfqUSWDVQ67ECZU/g9IjYWx7RyB3WliIClOVBWS1AWi4ffxZ" +
                "5IE9cJC0QGf1WCSGgCDhZJIDog/CA3Q59sAnKGzBiYCWg8yvL2BzV9lBrCSLwVmxJBMb+NE06sAtNoTQDxdICI2" +
                "+WDgJSn0vCqojIPFYE7A04YHIDVW47BzwtynIBeY3bV19yCDzxm+TSCAxNkUGcsdHceqFiaYGRXz9RKghUuzyN7" +
                "EoIx+wBH9dGP0z6SuOWA68bQoXU0/CVS1Y7CJVL9TjIvC7Gogv2C2dUTMyzmbJMOovA8OBN0hSazfGaB4uFQGR0" +
                "M8RqGo72rmFqhyusvqXMhBqleIm7SL2yEBNK4rUBjXap21S4JcRDwa5EPWanr1sq5bZVS+YpWsJe7RJ1Omqfkan" +
                "OA8u580Ym5nl1WQSi8YxHfCtHXYwWDYFXIVbTcvJiB1MIEC+LOn4hVrs61Es8zDV1OpT0zzvEgAErk7lWb+F+a7" +
                "NLD3ZOQ/TNPJm1I9heu5QWEDQ7OjA15dIrzF45Qy9IfHyAEZX/bmxZBJJRUg3MDS28O1wsBEbeg1iNufcMUxvci" +
                "Dkps5k8Bdrhbl3qmEfEKm+HrZl2xFHoATvgSkxR8OFpwYD6im+x0lUCYr0/0C5eLrApaHaGbM0IpXnpZirVeADf" +
                "ysAsT/4+uiwC0fgsycCNCBsIRD58svDlrMaPMrS9Qge14yY1B13y5ewcTrXKFzDSPWcRPbiHl+90FhpSF6JgZxj" +
                "h0/WFm9BOuySfryFQ+73tVLMrPBaGchG78X0MVBD+1XAeCMybQjNJNhC48teFu5PEpjCEgM6npjjHuEosmFcbCa" +
                "fGHHbT7bjiCcDyYfRIClQDSjczirFc4wu05UkNt87hN1PgX27GGIqM+s9d9MLP8p14PgjEZjJ1EkNsIDD6nYJXo" +
                "7xwcT0zu9uAUcULRcaAuzHPw4fX4vNrmYk7SlFdYQggVg8jpTlVhOqry//45usCDoqcdjXmZKZYqPz7+F74kou3" +
                "YvkgEI1kTKHJOAsIDL9Z6GK4u/CjzAhchYXKDGpwBGmUTCA1jpCNEdO33FFghljsNDNRlSC00SyWqpB7qWrM1Vg" +
                "7MwfV2gs9a6En+Vl+CCSjZMHz2FRyGQT+RNUUmT8CIz8r2Bm+2L2Fmb2VeymqWRrQgJqWKPL1QQcjDLS7Gwp6Tu" +
                "VuRpggIKineeBymrwusB7gO1BmUo7lXfhZHeQcj/8hmRcC0cG7pBr4PJWLwKepJYulc3oB8kbgo8cLW0z9MfwIM" +
                "0qg2uGppiJATvNMLdcEnUwwUI/qCxJls5cBq0NkC9CvcEOavG6gHkCaPQZmjqqad/CjLXBTPE1NQC4C0dTVTH/L" +
                "YC4C8cElW2YKQSAy8nRhcvsGflLFzM5qAm1Aq1fq8stpRzO46iDKAAPlTrQQg0bksMNHTfhWv66AWZBGr6sRaCQ" +
                "50Dpm3hR6Bj+FQWk56tzwgwgkpzKm0ANaID7fPwBG4ItCECgsP6z58cC5tczsq9jhAZq9EtTfWsB2c1QBJwMFmY" +
                "aCCpZkbvi2Z57Bry3InyAYAMo6IvMypAbKfMfxnj31EN7Ot/NGIBrPlIzORh5AIMsAgwhc+TZ9KRNtP4efeYah1" +
                "Ls6CFQCRHtva0G/AVEHLfDWSLOrrYCn9L5a2F/MUfhNBRqYiDxoASUYK5xOMUMM1B+8NHBut04s4BU2vpc/AvOm" +
                "0FTsAQQyDGQQuHY5Z4xFC9IC36O3CH5127YzA/1HdQxtqsDmBSmBCkNYXeCBWqELaaDDMkKXtYDfbMWg3aNar71" +
                "gea3QBjUIUA0wFBQiCN14onsAP3PwtcLGz+P5I5C8dS194Xb8QQRIBpiLCEWGfk5rDa+8fgrHe09vr2ZqTxVBBU" +
                "DUEUXIWLA0VVmDjdCv2uakX3RU4zTCugJ8u7sJ4jyx+0ArF2A25uq0uesPvX++u7ew8b80EIjGr398X+wXIpBmg" +
                "Lm8QGT4/+gtoufCr45sYa77qdwUAB16Eq8LArI6twv2GyJqVErfesLkkK4AR+2DOrAlAXsVaFYPo03LXOumbYWN" +
                "X8ZoIBAdIsV+PJJ8EAGCAQYRGPklvUU8jzHa+NHgBUXAK63BJijVEtDDHseNLvo9XtoAbOil2muDqvDnqDqUAFO" +
                "oJmBGykph/CJJB4HkXdIUmo4sQiA6yCQC//4wtwRp7wQdti2dOigRrjSFYPulRJiZ9mJMfj6kEjBiSwXey/lCYZ" +
                "1UJBRQ582qXB6AR15hc1aXBAK/jdJBIJrKdM9cjy1CIDrEHAKR/3yYWyKwgfqahG6M2iWs4AtF0gbhUnJA5BQ8d" +
                "h6kNLocdH1qvs0KWUQj8VkofykiaNC6Qx2hQGcYMzaLqD7XvKp9sTGGtPgVJYHAR/QQiA6TyYFr8eQiBFIMIvBf" +
                "D3NLmgIqwAtrW01lhHCqmgyuUGcg0BH2GBqFlJY3ogs3IZAHMt2DE6l36+AiURyTW0Z5vMvdYcxq0KkVOp3F1RH" +
                "QUBwSfAcoMCsKmkrCEvofmggk745n6yQWIfA5cwj898PckvZVAOtZiKEU3qyIaI1xW/V6hUKvt7jCPqWI2iN2wq" +
                "kBThvaQk9qOM2eVjg5E2I2CoY4MjTkaK/PumE8idIWDsjBC2xd3bb4W1Q67MJSQCBKE4Fsyej4ZKyYCEQe4o5UW" +
                "jHAm5F1grsky2XeDruqjjcvB2pbyCOn8BkQQxCuOAyRutrohXc4Sj+kN6x0UTQbVKqCmCoH90qFqxOcARS6AdXR" +
                "XI23uRQQoKsFiJ7hifTViZFHVQvU+wyLRZhnxYA5Vp4+7FbleA58hSuso4igNGJ6uAClCNPSm6Dc0AF30hKFSWB" +
                "3mm8M2RZVroqNHQ4BMJy7BiDtdau1K9EXILpnJklTaPbK7eIh8DB9gTbQ26pfZQDJHs8Ytiw62GvNITvYGCq3+e" +
                "DKJCq8NEvehCYvXECoDjOC1Ys+aAIsskLXYQcFqBtDgLiowNteCs7Ab2kjkPV7x2euFg+BhxkRUgZqANGLAKjVr" +
                "9wYMgFkulIbsIHDMGp/E9yZTNd8llgdcCkTtQv8jRU++72Vy2zWe7kzni5sAhwWPIlwsbRzxVWlgAC9vEBmjGSS" +
                "A7MgBKZTKz0vIKipAAT/akGSpAma78mBymZru4+GH3y+SVADVHyGa3TS8yZkTjOUB444XEC9VYO5s9Gf2uf3njh" +
                "//sz3d2a1Bc8cai6NlFe+419ys8OxVCoVWw6BwYwpNA1EIDbIAAL/uiL2rs43n/lX7eg6ff78qX3bskkkgd3bCL" +
                "aEPFASyW1HZbQeaEa1UPm8KnBlHtcYyH6P9UdxHH//7CUcP707I/k1Lg9vRSHwk5waofidy9duz8SWQSAauXqvG" +
                "HqRIZSKp4ZyxkgBNUI/WQlbx3FgWTnffpKQg7NniR/Ht2Z9BwycBtP5oSqXiagovSCKCoNrFpCiKrBfb8uQteUi" +
                "fmrnOoM2+PWDOL4v43aoAy0rSg38ML7I0x2fZ4ASgWTqGhUCY5/Ozn66cMzeHaFfKfrDlbB1DR5j5lV/F8ePbOv" +
                "UatdsJ47EXRkDQo8BY4ktbqgwPdLsptcWp/fLoNaoRIHazBDO1Hps6MH3arKKb1sPvo9UilUObEWpgZx+gUyck7" +
                "Dnl0EgGv+ECoHF4+ZQAf0CK2DnEJuLPNG5e3B8Z9ZCbyL+vYP8V7XXCjp+a516KGegDtPT+rzWB4eA0QXSWlUOl" +
                "PSXdOfx1+7bWRsu4Nuyqq5uJSGQ0zU2mhHaa8PLIRCNTxcTgQK6xtgfIsxCHujr+/u33b+6E+8JkyaLGdguXOWw" +
                "V8AhQC+UbgrAhaDsKOjrNmXMK84r+DsLowQ78ffJqp8ml4azghDI6R3OFAB9fHlkWQSS0aJqgc0rYOdaM22Mdcf" +
                "wVxZe3o8fFpJhVBeoCIxjgfMV6ynC9FTDGRZDBoRAdlt7pua1+kKfZ+Fl/jF8U/pvoQO6S4fN8XQyxxcgI/53Yw" +
                "vNnYkhUMA0kikZ/Xg2/kBbJRMIpJ5eATunQUkbIYCfybEVZOf6SVul2mMCPWXxQSEg8DlpfR6FSw5XOY0gBPSdZ" +
                "FZsDX4q9/pu/LWMQ2xaSYbQ43/IZWBs/PKdwXuNkteJMUnRVDOZvnl95t7jsbvXc8ZkdkzlusMjU+mLM0vGRD96" +
                "fCUgkLHKd+BdudcP41vSYlPhswGf8kCFhCp8LpoIQLWqVTu1AAS4Znc69oPs6nst98a67rPbnt20aePOF7dsWkE" +
                "jt20sNjw0fP9CMpYeFIkz8l5sAUCZT98bkaFhciwS8PT1kaWbxlbCxu3a/Rzxtree6NuaKwc7+/ems0lcGzAs2o" +
                "ZB9c3wfCibCIicOgAC5RYy5Yzs6dmVe8N7Fr/U09fX25f5D350f0CMiz3MTEY5/hiLFmkskyNbIi3wx6KstLfnY" +
                "npHu5mbMP0HP78uVw6e6z2QFjuuyQ7KqyqxukcAAWcGgd49uTc6z3cf6jpwYP/hI68fYGDs33ecSLXgAyfe2H+g" +
                "mOOn8WipMTD602IsdP/rx3vTO3pqLzMb+vbbxI+uN851P5srB1+/tF9IWgt2/iOKAFH0TSYANl16J/fG9oGDDVV" +
                "CoRBbJREyMAQ7u/H+g2vbGkTCoo7Nw9FSY+DK5qKsVFQvD7+O473fFVQxMZ0/WEP8rH1t4KVcOdiH70rHQyowK/" +
                "KIIlCmC5MOjeLShdxc9ev49oyvYCxjYAhf6caPoqKyoo8nfx8tNQY+fLJoqxWaD+G9XQ2MuMOZU/4Z/GiOlJSfw" +
                "v1kMMVrKXsEEABGhFq98nTgn/f2wMsLb6/r6SaDpPUoZLNmxhXvGri0k522sndjJcbAyHvFXG7V1m78sJSBidpc" +
                "5BGoOdv//MLLO/pPNqaNhUZwKYOGbQSqYX4d14GCklw1qInMmK3rv/jsfQY0x/C95KdVHhl8kRBnJ/7BRi4rBJS" +
                "9moqWFgOjrxZ1vciaX+EvM5C6qXORUVFkF356gVh2vj+wLf3+Oe0okDMzXAtLJU0EbCE42u0oSAq5piBJVuXLAx" +
                "9szDr9HNMx/LiBDNxavVXw2/vMQP92tuLbm4uKQAEMFD03vKEP3wZ/TJU7neSbbnkbP+3NIsVb8z7+OnnM8902U" +
                "EwUMcNlh2s8Flqf10HWCOlRoBaRe7UkGvx9/fhei1QklLRtu4CftJM3mz3t8Kd3448H9rGW4nniN0m6Yp2MEI0x" +
                "qdRgURgY+fUTxV7y7v6T7fDaRJE1ddqO4j27lPVCYYNmTz9+uJG82eZWgh7i261QCqgO07GKgAqsy/hWb8aeq3j" +
                "pPNEpcKCLKJG9+E5mTwU2P7wLi6zHz6jZS3O+RdMZiA3fmr1NjM8mU8VgYOTNoq+44Ri+A14N8FyomPxH08EPiE" +
                "6Brq4TOH6hK1McJ0KdwOOzBrpSlB4C2ozjWvBocoGLs2XYfMg3sP/0BSKHdfbopqxB1O7XwCsBydv9L7FHQNkL9" +
                "Cyh1PXLieyYr6djlIHRbxV/ydv7j7fCn1StnvlysPVHzl681PfBmcNrs0aSEWsDPtPiVkCJZCPNSlG5C7ZrzAQ8" +
                "KzjtXv38xOVrtmzZMK8sOHIvWgn/gsz42TYWEXjqd7QImE3L/s3rN25OJ+aGi+APfPhU8ZdcdwJfCz8L1+JVzx9" +
                "44me2bl037wUiSo8JbPK3+2qgwiQKl5zWAy2oHgoBxIGC3Xe+JaACLJEj93tqGHhBz+DvlLE53qSRIE5dJQCID8" +
                "eJIqDh4ckY8z7x0I/YWPI+nIlwQ5XTrwJIWHmLlyKjw7XBFYpyVGgLrQekdhvUL0TUKEVIqdYW0C8K/PDaAh4m2" +
                "mW4L+IvsYrAN0fyJiB+PZG4OjjvP8cWFMjl+tTJ5PzfmRv5M3Dlm2wseTveVcHANGJnoH3RKVnZHqSUGydcJT3X" +
                "4KQX5BSZXVBB2DIJpfMhNgcc0hy+ODXmDhcjDWPVb/RvYRWBr/0i35hQ8ouJxNjog+5xfCZd/hyPzfOQTMZnZqb" +
                "IroKpO3fuRuM0GBj5t6+xseR1/ccaGXlX9rBNkiPV5RJr0EYlByp/E5QbzrXa6UVbKgwhAVzoF0Wp1EilytthbK" +
                "jKmoIcgUQTCpmqGXk94kO961lFoOzv8rWE4jcT4w9aP6lb0xPj4+MTt29F0v87NTc3GZ+emCA+Fhv5fOzatYk5M" +
                "nKUJwNDf8vKigMXTzQzMpFA5w/rJFVZH5fDr9OFA1qqtD7H4odLGnE9NnrKi6MJiuEWqHJTex9Sg7/TqW1uIEaT" +
                "2r46ZG1FmHk94oN9G9hF4C/ztYRSc4m5+CIqEuNjc2PjicQnRKIgOZVI3JhITCRuxJIzE4nE2Bzx47P8GbjyF6y" +
                "sONx9XMbQVE3mwBqnhpQDmcbeGTJRTyzF4EKiZQKPiV5AidPqhURd5LVRf2euVGP3da4hxuqgUyfjMfV62EfgK3" +
                "+fX5YrOpwV5wVW0J3xxNzkVGwqLfe3kmkExi+P37hLtJOlxhITN6eSd6cTiZlYngwM/8NXVhoCZRWN7fbAKlIO/" +
                "A6tjPqYRoxhCdyvqnepaSIgdasgW9nN2JIQVdU1y1tb5a1SJks62Ueg7BuR/FwB4oi/nktLaiIxN0j4x8k44Sbc" +
                "TpEIjN+JEO5xZDYxPhMh7oyOJaZTefrEo98oW3EIpOVA2pKWg5b6JSsb6zEnZG1Si6uVpkCLHCZIv7/Wb+WyLI4" +
                "PA4HH/imv8GbsViIxOZirBIgrGdMo9XliYiqZpuQ2OVlyLHF1KEkMwlQa+yK/uNDIu4+tSATyCzAaOhrhbGWi9o" +
                "5uwIXIXUBmqjgWrOVLgEDZC5H8AkL3BH7eE/gsMTEfCZpJjBMqgtACsxESDsIfuDVDjFs3xifuxPLKD7CRGX5oC" +
                "DR4UEhjGXE4acuzMQQbpBH7bJVfAgS++l5+aiCRuJmLwHTi9uA8H9fGiZtpdzj9kdh1IkqUHYl5BJZhYOSfv/ro" +
                "IsAzh2HbFCo9dtrPKL2NsN+83a/+EiCQZ6HQUCJt71MhMJH4hETgOonADQKBMZKAsbG5O8l88sSjL5Q9ugjIwyZ" +
                "Ix7Ss1q2l/YzUpYY15QVuj5Tdvap8o2892wQge7rzUQOpaTLsk4PA5XkEpq5lDKEsAoQWuBNLZkZ+tRIj//gYW+" +
                "v1dR+vY3eHa9x+MexLakXpW+VChwM6Dy4NOapY2iaeuFGmVDmO9G1vUcikIh5r76f6EI7n4w0Qcp34fGESIf7p+" +
                "PhURsJjk4mJL0h3OINA2heI06oXGv0bFpZaIWyQyWUbe076FMrmxhq2rFy+ZRV03ggxorX0HzL5BNAHpCakLy/+" +
                "HpXXKfQW1OML+dcd7XvRGfR5HBa9oobDxvvhHsS7N/4gnxTxIFEofa9NIDkYTRIxopsZJoZuJy6novcQiA6OJS5" +
                "naynieTEw9IM/K7auE8t1ZgfmC1p39JzegIX9HtRqUDZUFH+HOZoOI3RosdxpL0Ac2rxN0DlbnjmkKXJklFOvtb" +
                "r8YZdR3SwVtRzqe65K2qoye8J+1KKRFJ+AF/Fz4bI//488KoViyQki4vP/7Ft7cFvlldetXtVbsiRL6EWtB7IVV" +
                "e8n96H7kCo7DoRtwqPplmyABiYUQgjQlClLKKWBDM2QAG3dhknYdDpxOnhnmTBOd5p0UjPUk9bJ9I+E/WIQ9cR0" +
                "JrNQll3YAco+ZLCle69sx766V1fV6PtL0rXvvd/5zu88fuec1yvVdWHm02o5eI4KvXyhMl25UO2d+HSaBoHKO2+" +
                "99V6lMn3xwuvv/fcKMHDpX9cLnGP5EzmiVEgG+2yadZOHQ1V34I9miySSCtsEtnGQr5hRN30XMxzm8F/WfKh5O6" +
                "rLU16BXUCijMbderViDmraPXO5ACRVaIyuGEHl4h6BzdTwmTNzfXk7VxIKXazGN2998uE77/zn+x+/+6c5f/DJW" +
                "+9+ePnTy9Um6vfmGyQuVxa6Kd76+PJfL3/0yb/918yVMfDGfYLC3BnLEmTCY9IqZVWjSL52aC7DkypUOkcEKWU0" +
                "wkrYWeSji96X55LCy9MoDxG1IU8IWx2APA61AmpkhCCF1p4isYxHyHhI9yzYPQe9Nc+vJCOenv7gL/NDY3+ZKwB" +
                "Uv7/7+bd/vzBfQJ5PAWZm5q+8+6fpK+cDb76wRkD5OpIIkXXqazKmM0JyldUrbLxpx1EefLk0A3NCaoiXFv4elA" +
                "gK6iulS5OiSmOIDAh5RNefP/F5M+CmV1fUND376Ud//vjjDz585/PW0JnKf7z/wccfvPc/8xH/R+99ujAsMHu5e" +
                "uXP7//1wgpy4v/9RyFNjAuP9iihJUlRQREg9eIIH9GsAU5wCuqdq20sWiKOzFLhVhE0DXUBmVHIR2u+d37X/Med" +
                "K2uanq5crM7IVOqTM9WvFysL3yqVGdpfzl2ZXgEv9Pb/jQjJPctNTAvWwrqAPEjCvORzfbCPEwTUmTwv5tuQIdI" +
                "mkSAg7LJOnIzMf7zmmVWMUDa5mBh44/nRc+kWbrp1ENAn8Bw/jEYC4djqEEF7eXkBZYhAPLIOhMAgOFL7vP6lGV" +
                "EwcOmf1z8N/r4DISB15omYmpdbmXJpjupny4f5ifSgPgSPG6SdBgFo8/nv1b/dOfu6GBh4+07JjvPbFZ0GAUgXw" +
                "RA/T3bTj3CtrcnTCF+MlymJFYJqaWdBQP7A5Fba14fEwMDbD0kkN07tMXYWBCClF8OSPXydUxN6HIB5IzSl3lwB" +
                "C2jkHQyBq5+stBwDbzxRbRDd2GEQkKk8CJYL8KYr1jz3oUtjPs1fCK/qz2FEUNa5EJBc+9zFFmPgjR9dK+k4CEA" +
                "eDMuG+Wsug6KFXu59Dgm4l8e9aQLZgLyDISC57sXplmLg0j9dJ+k8CEg9mX5ep2mzzTCbdiTG6+5USqiTISC5+Z" +
                "etxMClf7lJ0oEQkMjUvN6uv9BMo6ksA+skf0NLbAhIbn2ldRiYfmW+Q3pjx6XDfC5FBmsKUm401IXAKiAgua1lG" +
                "Jh+ZeGZXQgst3x4cz0y6nxe24XAKiDQMgxMv/IPXQisxAmk8SaF48P+FtyAwSVjQkDv1ooEAcmtLckHpn9ZnxNr" +
                "DQTUzh4WBBw9svZXDR/RbM+/Gml/N6Dz4WU9EwK+dTmHQhwISG5+UXhutPLizZKWQkBmS68LMyGgwMl+Q7vrhjJ" +
                "NNv2OPrzd3YAtP5j1aJgQMIeIoYRBHAhIrntO6BpZ5bm/k7QUAtpImRywMCGgdKdLqLvNHYGXbH7wS43AbQ11hY" +
                "8iA7rGXKA3NoTaxIGA5Nonhe2VmH3yWklLIWDJDiV7G3MBbR9a7Fe3s3ZosiQP2uukYu2MgIHBvG3RdFjhpSinO" +
                "BCQXP3QrHB9ozOzD10taSkErAUqqFo0HTanytF2xkBw0M9DJUqeQ3vbGAHlnGEpRshBkE5xIFDtG31JqGCo8tKd" +
                "rGcJDQErSvZBSzBC2mSxjTFgRDBeGj17iykVp38Uvjsa8g/m6LU7FilqJ0izSBCQrH+mIoQjmKk8s17SWgjoEcJ" +
                "D+8oiRdXJUkj4fADiZMvlsSEXL+0I0hjh5fJ/hphVaMnYqTyjm4RdF3DgqFYkCEiu2fkq/+zo9Ks7r5G0FgKKDM" +
                "44fnZdQJOm+oQ+Z1Oc0xi7g0rzRAvqsRwHEUOuMhZQCSoZHUwxRdNQGgsWo1KRICCRbHr+wgzPLuD5TYs8R1gIB" +
                "AimCBtKY0YcFXYuVu5FhgMcrLk2y18Q4C2HObyByokUkw4hvWNwiDUVLd9z/gamCUtSDfZD2ezoDtRngVYCAcma" +
                "nb/iMyOo/GrnGslyEDD2NWn0IHWDzTKgWWasj5w9zCJZnKWIkKFQb7yI93HJNwJDA7x1ZarSBCdV1oWKhYhFMNn" +
                "0FGBWsoM8dngr0yA60BTreJTheHPBkSNOppUsCOiW4F/XP3GBr2ho+sITzCzAamRBQBYlE56m4K2KxNnpYwT1Mc" +
                "W3+dC+YZZEMwXh6GdTCCvFODk5CwHzGAWbiDwnXye3potwWKCUVNq/lpHsBLYfGj8zNXF03y20H2VxgpWRSCOlZ" +
                "hiu6kxrKdIjZUJAFiKWmPH44q0/nuUDBNOzP771iwxtHSjEFUwIQMYQVYg1M25opJKs1noDkqX/csPeI6enpk4c" +
                "2pVhgHEwKhcKAPlSzs7Jt2mzRV6B6S3GuPlYpQMu5WN2QTIBZgvflmMAnN635xAArx2I0Gw2Ema5AedwE+Ogtmw" +
                "pbZGzcwHInC9llkj+r7rrhaY9wfSFF+68iqkbKQq1Sdm5gLwnXsw3sbveYT+bckNoubBj7ykAjuzZOw7A8Xvph5" +
                "xGeoQ4YVskV8Tc3PJJWX+5n9fhFGWs6OPqXJ05EskJEA4519LLHtsmwP51QYvJGb/jNDiSr4sixW4XNxeD3J+Jk" +
                "X7lYumwqp/CljI6V9/1swvNtA1dvPCzu65m7QGhIqrF0mGFBy94OW+vr8QKeGWZfF16/QfBubtTLlOPn3gMnHuQ" +
                "Zllcgz7+z1efhQnYreaox65iiud6hQEucDblSktaAM4AGijS7O7tr4F75mEGpUfA4WTtykCBZZ71cIorUeUsFJz" +
                "SxRkhmRtfEgOSq2774ZscywQzlTd/eNtVEjYCcL98cUYIsqI4VwwoYmgP+9jjNRX0PguOEvNPNWyZOre9HhcYiB" +
                "j/CbE2n7WpuBpyM4bxHn/bCC7M6IJsNfzHipoMrSqWHAf3KOvmagTsrmmqA2aNTKjTOY5pkh0vOKClSFHIXSgsX" +
                "Qj5wle/+/Kl1dNDlUsvf/erX2gI2dGCT7okKWpBcY5EvSqVYw0JuuE6nHaAo/UMANo8OTVM40tyet4PWKbhzm9p" +
                "05Sb9xldyFuKayRttEw4jYp7EOyln8HQ6XPDdX+akLHy4QI3n2RCGAhgk6KQqwAvpwhf/uaTv52dXrkvmJme/e2" +
                "T3/zyIpqaYSCgoS5gQTFuCb8OSbC0bgCu2dLMqbM30qW4C+ytOVMoSLRVE40qUhoQYEpdHiqG5W20TWupHtLLxw" +
                "DOwOsusKOeq+VYrz1AcEreVHGqjyFXdl0A8hejymVvce1dT/16dkUM0fTs7K+fuuvaxRM9IsR4j4bSmINMcfJzO" +
                "izG4lTj9dnxLWAv42LfyXN1D+EpudpINWTBYlwpCAOTpnxQ++zTPlyXOnnqGFOrbzl/oFa/SRZYpi1IcUrOXWU2" +
                "t3T/5B2sWLp0xYTpmk3f/unv3qxML+kOZqanK2/+7qff3nTNEnfoIViJ3oap7zP9szRQ5tQgqSciLAhk4YUfVLv" +
                "BNua1A+duqTueYW/7aAbkIrMC1auNOcoDtSUEtpx5jKkEuVPHY0tCwD3MpYFUm2PlilrHw2fusTC0sRdLr4SGWH" +
                "PTfU/94g8zs7OzlUoNCjPTlUr1l5k//OKp+25as4w3jmGM11eat0ztCzIjMD2CcEnczGtZnCgNAp7DU6yK2Pap7" +
                "dK2hICdgAXrTrMgBTvUjhC4fephpupFx8fjS0LAyQUCkGuIwaVa1u0+DgA4tqtINzjh8op5sy99ZdO3vvODn/z8" +
                "5d/8/o+v//H3v3n55z/5wXe+tekrX7rC//UQMbprzu84Wn2Nk3s3MuhMzxAXN7AcBPqOvEYwr22b3NGWEDCjqF2" +
                "4u9vw9sGAnVbh2jy5h+kFsImxhXlPKImxIODiAgF1BqWzbLkRcP7ksdGxaq1oD40T70WTSmG3LQsjtLDb/ki1Hj" +
                "g2euzEOTC6ju4G4LyOCwQCrF8y8ELs5zx09nrmtQem7q1BoHdt20DAguBuIe9fJcbt7bLVYn/tc+rUCSYL/A3wt" +
                "Kqmu1lWOhwscfCTPaUwLU4mT4Cx7dUMXDa06zg4GqFFKaTAY6YqNF2vajhHwInd11fRn9x2FNAJGygwyOGcjMUI" +
                "65dYjeOSPwzuYV47BOqY8BadbYOAgsBodBYIR3vs1ViI1rPTUcAwUbLHwd0Lnw05dtmGCyME9ZVotgU+AfYtiCF" +
                "6GIzW4we/0LpgwUN1UmwEjFELqfgj4OwGmjsaDK7eXTcyQv1wjTn4GhhhVBSTE5M1SyLtx63toRVmtCC4P3LibY" +
                "IBdYrGwt8NDtGJ6RvBZK4W9iAsHgsK4auHgDJOa4MxHgZ1wkniPQL218IwKyIscyz1wzXzrnwEjNHa1R4B4/WSm" +
                "D6fVnOAQJzdTwXX0B08DugUsGIveLi2U3U2q20LpbC0AAFV0rld/EBwsK71rlHwaF0piWPg/nqGirMq5aoEvPpS" +
                "piZPC/K3nhun06rhU1O31FOGjKBjQrJYtrbP8sRrZfq1g+BBOm+0ek5Ik82wqp86OFmzH1vBibqrlW4HpzO0CCo" +
                "qbQeVMLcEAZ/HQu2QE9uHaL7++pPg0fkIAbphDOyrhSOKDMpKULW51OpT1iplXv9yAGxmXLwXPFpTjTgqaA1dnk" +
                "nLaY9lKN4wGK17oACH4ociBrNwI03Xe+4Nu8Hpr83j270DnK3LAPIM9rUJAnyteZIDw/1tgAFNFqOxHhtPgGNbh" +
                "/q96Q27ANhfJ03c2ADLQJmp8OpNVs9wnSzxjE0xJY2DIzXVCQ/qhNy0gqg1AGr3gxuZ7u3kaZRGe63eV0sDFPuf" +
                "vEidCtY/DsCjG3NeP7X5IDhDi4qU+byxDRDgapEP+CzXypeCCtF3DAWG6Q0L60aq/PjY0SpLeeLhetguT5LswN8" +
                "+zMFUmGh84Q2Th5h0y8D4aXzhc6gkLAQKNQhEjk8iTPf2/cktdegPc6CEnA30vhbJ112m7P4xACaOjlYFfejrtI" +
                "OwD4XFj4NkHhJtIS1lypSjOtE3bUAxehKm/8buI6dOj+9/gKT96C7E2afjG3I0B4ENZ/cwmU/bwcli6yGQPDnGp" +
                "PFVD0zd3hwEesohtnsfwOgVweyOp49PTIw+fgddhMo8ZhFdGaoDG/mWturp4qVMj+jb9g/1M/XbiWJpxi/qNMnu" +
                "FlGl8hwae+mB0A1nDjInAwJjE2TrA6HY+ESWGRnuPkP3AhyAXq2hsJkdLcoqsiULBVYNua8cFj0u1seL6RYrpHq" +
                "ARFxib1ydKy8/HgqFGptmq/UfDl7bQIVrn/vHJz2Miwg4umCAoHhBUHpQkUst8JbGEbCBee34RKkOSy71PyjYOF" +
                "fdRyaWp3l7SET0iKA3V4zqW/1QmQejgmqRd24lsGWh30fmGihKN6davgamtdKPgI2Mi1vB4zUfk84IKhR5LFs76" +
                "h1gF+MaCsZqcbssWuCSodrXNlTUZAkyuJzN0OcJsRul5V6CCIiRnFozpYRZ5M33lZBl2mLdFN6gB8oUxsVhqpI0" +
                "0uMeMEpHlmP8fI0gNMNRQc9CGszXYvx1U6fS9Gv7wCP1+IVbsUqHZhs4XW2eXIYA1GWKQZGjAWOkhIhE0+tCRcQ" +
                "j7hSNzF9EzEv7gEXK9j1lLnGQBPLROh/sY2A3LUY6AA7VcOgtuoU9jF58oKabe8ERWkRWLVbVaxcWbmNTi/YWmf" +
                "JUYKnRYEOWCilFVQHImR9MGMV6utyFklGzyBgoIZ7F7fYAhTdSBLKBMje3bR4cqH9ZdwY8umDsrfvByVpaCkVKA" +
                "p+GppCqWR3/MXA4VEcAqCfDEu9abgShtRxtNGompBhZPNy3Z4shcelxQ5givCoRX8AYKyEBUa2AzEWQkUZOTupM" +
                "D+YWiXgMZIqbvDQwTGNCN5wDB2+f44hi2w6Dk1Tdx8BZoZul43CdAoiNgtHt+eoH2y37AL2FR5NFuXWsymOlRQh" +
                "OQ3Iw42qEhrEfI72iIkDmhUtpkaNxpQulMnZRX6EnXYQHzCy3L02uDS3SqSD1Fz0c/a13Lb2idv2x6rjMs/tGqr" +
                "WiQ7QCVXBQ4DhIAlmpaP1btKr54yP7Dhw9B07TmzYca/s5FqtspdAisa0qUGZPYEsMwRyV6xW1JmZJU0SfSiL20" +
                "oVxcSEgUTnhIrsLQmKyLha+GqkUVyutxxgEeej2uWo0OPv012ntMUYY0Qi9W0UaoWU4to2PT8y9x+FtKdrfqDM4" +
                "V9Moi5UXqzBJzRaoIdXCvBpxT95MJPTt0K4m18rFfgW127QiSSiiFOeJIsg3xCzG2pMkScTpqgaFBHcCcwlxkdH" +
                "SbIzgJJlhbsszFOJsnW3F1MpoXZ1bK3ZbhEynkHTXvPKt7M/spbiMeyKaJZYvxElcZLoFlRJ5DL9Ck5MZz3OfXZ" +
                "OGBj0Qn0LvrjZaOhRpJnXqJeBlSwrVqdWWZGY6BF22+UGXJZspVhnyRE9XVzpzKROEtxnLBfmo/DLKYc1T7tYYR" +
                "hsBL4MBfYYcaCpCcRIZTVdbOnHJ+4loc1mLNEAsjQErTPhaFBpALhxZEgO6DBmWNXf7ASqq7OpL5y2pG083284F" +
                "+YklSvFSN9aci1ndezgLmG9RSw9Z83hI1uTt1QnKL+tqTMclzC4s13w/OeRFC2Fdg/JJ9TEc9bQwPYQcMJ4wNrw" +
                "HpAliaH/z2mvKEF0MdBwCepF6f1kzy5rGsH5WNUYTKmCpFtcoDQmsEGV1wim8CJp18HF3c7bglna1prOiICxn4+" +
                "dWCn8WZyUEvWTW2/LaiNSTJVlFDi2SG+CJlTVnSL+8qzcdlAm7Czk7b3fTsGvyGnGK9HIPi/2XufmbXaliINDNi" +
                "TtmacJ4xtYVw+pWT5JIGLpi6IwgyJLGk8auHFZtOKJ43t4NhjrhJP0kFlJ15bD6JQsgVEjblUM7L4P9iqqtsuWL" +
                "GXdXVNxWtRMQdl2xUqy0dhsqxFqh4ZR1WQZEbUuV0QF9V1JclzoIl7POZT2Byhxbm+x2zImzIHucGEy59EsUcWR" +
                "GT7qMx3q7gmoqKw6jgzmvaYmmZKnOkShTSVe3hiBapuuMoGsLIbtJzR4aU5kcIXIIidi79qnZZQ3lB6mos0fN1n" +
                "OVwdYPry1E3d2mfVGX2Z8mh4lUv9Nq0qs/WzqT1TWQIobJpM/clQ8fy9iXwIepdMjdOy9jld5kcQSSheFiJmjte" +
                "gDx41WbLwqTwyUcSX+2YKI8TOYjPluXBuJtKS194Sw5PER8LuMUjJeGi0jc7+gSRm2yZAaLKxBKZOZWNhEKuKy6" +
                "rm3iO+jUWpz+UDz7mZBT4eD/t1uHBAAAMAjAItzh3r8mNRBbin1OgAAAAAAAAAAAAAAAAAAAxhR57H+jR+XqlgA" +
                "AAABJRU5ErkJggg==')\"\r\n\t}\r\n]", ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);


            //assert
            Assert.AreEqual(@"""Picture 1 successfully downloaded.\nTotal pictures uploaded: 1 from 1""", response.Content);

            Assert.IsTrue(System.IO.Directory.GetFiles(@"W:\Upload\").Length == 1);
            Assert.IsTrue(System.IO.Directory.GetFiles(@"W:\Preview\").Length == 1);

            //after test
            System.IO.Directory.Delete(@"W:\Upload", true);
            System.IO.Directory.Delete(@"W:\Preview", true);
        }

        [TestMethod()]
        public void DataProcessTest()
        {
            //arrange
            string path = @"W:/CV/";
            List<string> pics = new List<string>()
            {
                "pic1.png",
                "pic2.png",
                "pic3.png"
            };

            var client = new RestClient("http://localhost:5000/api/ImageUpload");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            foreach (var pic in pics)
            {
                request.AddFile("Pictures", $"{path}{pic}");
            }

            //act
            IRestResponse response = client.Execute(request);


            //assert
            Assert.AreEqual(@"""Picture 1 successfully downloaded.\nPicture 2 successfully downloaded.\nPicture 3 successfully downloaded.\nTotal pictures uploaded: 3 from 3""", response.Content);

            Assert.IsTrue(System.IO.File.Exists(@"W:\Upload\pic1.png"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Upload\pic2.png"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Upload\pic3.png"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Preview\preview_pic1.png"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Preview\preview_pic2.png"));
            Assert.IsTrue(System.IO.File.Exists(@"W:\Preview\preview_pic3.png"));

            //after test
            System.IO.Directory.Delete(@"W:\Upload", true);
            System.IO.Directory.Delete(@"W:\Preview", true);
        }

        [TestMethod()]
        public void uploadPreviewTest()
        {
            //arrange
            string pathFrom = @"W:\CV\";
            string pathTo = @"W:\CV\";
            string fileName = @"pic1.png";

            //act
            Previewer.uploadPreview(pathFrom, pathTo, fileName);

            //assert
            Assert.IsTrue(System.IO.File.Exists(@"W:\CV\preview_pic1.png"));

            //after test
            System.IO.File.Delete(@"W:\CV\preview_pic1.png");
        }
    }
}