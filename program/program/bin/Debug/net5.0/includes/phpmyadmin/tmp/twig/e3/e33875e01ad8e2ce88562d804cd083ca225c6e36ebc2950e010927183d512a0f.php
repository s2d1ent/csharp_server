<?php

use Twig\Environment;
use Twig\Error\LoaderError;
use Twig\Error\RuntimeError;
use Twig\Extension\SandboxExtension;
use Twig\Markup;
use Twig\Sandbox\SecurityError;
use Twig\Sandbox\SecurityNotAllowedTagError;
use Twig\Sandbox\SecurityNotAllowedFilterError;
use Twig\Sandbox\SecurityNotAllowedFunctionError;
use Twig\Source;
use Twig\Template;

/* menu/breadcrumbs.twig */
class __TwigTemplate_5fe70034dbd32e0f5f85dc7d657a77e830f50f29f032fda0d05a56ccb063186d extends \Twig\Template
{
    private $source;
    private $macros = [];

    public function __construct(Environment $env)
    {
        parent::__construct($env);

        $this->source = $this->getSourceContext();

        $this->parent = false;

        $this->blocks = [
        ];
    }

    protected function doDisplay(array $context, array $blocks = [])
    {
        $macros = $this->macros;
        // line 1
        echo "<div id=\"floating_menubar\"></div>
<nav id=\"server-breadcrumb\" aria-label=\"breadcrumb\">
  <ol class=\"breadcrumb\">
    <li class=\"breadcrumb-item\">
      ";
        // line 5
        echo ((PhpMyAdmin\Util::showIcons("TabsMode")) ? (\PhpMyAdmin\Html\Generator::getImage("s_host")) : (""));
        echo "
      <a href=\"";
        // line 6
        echo PhpMyAdmin\Url::getFromRoute(twig_get_attribute($this->env, $this->source, ($context["server"] ?? null), "url", [], "any", false, false, false, 6));
        echo "\">
        ";
        // line 7
        if (PhpMyAdmin\Util::showText("TabsMode")) {
            echo _gettext("Server:");
        }
        // line 8
        echo "        ";
        echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["server"] ?? null), "name", [], "any", false, false, false, 8), "html", null, true);
        echo "
      </a>
    </li>

    ";
        // line 12
        if ( !twig_test_empty(($context["database"] ?? null))) {
            // line 13
            echo "      <li class=\"breadcrumb-item\">
        ";
            // line 14
            echo ((PhpMyAdmin\Util::showIcons("TabsMode")) ? (\PhpMyAdmin\Html\Generator::getImage("s_db")) : (""));
            echo "
        <a href=\"";
            // line 15
            echo PhpMyAdmin\Url::getFromRoute(twig_get_attribute($this->env, $this->source, ($context["database"] ?? null), "url", [], "any", false, false, false, 15), ["db" => twig_get_attribute($this->env, $this->source, ($context["database"] ?? null), "name", [], "any", false, false, false, 15)]);
            echo "\">
          ";
            // line 16
            if (PhpMyAdmin\Util::showText("TabsMode")) {
                echo _gettext("Database:");
            }
            // line 17
            echo "          ";
            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database"] ?? null), "name", [], "any", false, false, false, 17), "html", null, true);
            echo "
        </a>
      </li>

      ";
            // line 21
            if ( !twig_test_empty(($context["table"] ?? null))) {
                // line 22
                echo "        <li class=\"breadcrumb-item\">
          ";
                // line 23
                echo ((PhpMyAdmin\Util::showIcons("TabsMode")) ? (\PhpMyAdmin\Html\Generator::getImage(((twig_get_attribute($this->env, $this->source, ($context["table"] ?? null), "is_view", [], "any", false, false, false, 23)) ? ("b_views") : ("s_tbl")))) : (""));
                echo "
          <a href=\"";
                // line 24
                echo PhpMyAdmin\Url::getFromRoute(twig_get_attribute($this->env, $this->source, ($context["table"] ?? null), "url", [], "any", false, false, false, 24), ["db" => twig_get_attribute($this->env, $this->source, ($context["database"] ?? null), "name", [], "any", false, false, false, 24), "table" => twig_get_attribute($this->env, $this->source, ($context["table"] ?? null), "name", [], "any", false, false, false, 24)]);
                echo "\">
            ";
                // line 25
                if (PhpMyAdmin\Util::showText("TabsMode")) {
                    // line 26
                    echo "              ";
                    if (twig_get_attribute($this->env, $this->source, ($context["table"] ?? null), "is_view", [], "any", false, false, false, 26)) {
                        // line 27
                        echo "                ";
                        echo _gettext("View:");
                        // line 28
                        echo "              ";
                    } else {
                        // line 29
                        echo "                ";
                        echo _gettext("Table:");
                        // line 30
                        echo "              ";
                    }
                    // line 31
                    echo "            ";
                }
                // line 32
                echo "            ";
                echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["table"] ?? null), "name", [], "any", false, false, false, 32), "html", null, true);
                echo "
          </a>
        </li>

        ";
                // line 36
                if ( !twig_test_empty(twig_get_attribute($this->env, $this->source, ($context["table"] ?? null), "comment", [], "any", false, false, false, 36))) {
                    // line 37
                    echo "          <span class=\"breadcrumb-comment\">“";
                    echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["table"] ?? null), "comment", [], "any", false, false, false, 37), "html", null, true);
                    echo "”</span>
        ";
                }
                // line 39
                echo "      ";
            } elseif ( !twig_test_empty(twig_get_attribute($this->env, $this->source, ($context["database"] ?? null), "comment", [], "any", false, false, false, 39))) {
                // line 40
                echo "        <span class=\"breadcrumb-comment\">“";
                echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database"] ?? null), "comment", [], "any", false, false, false, 40), "html", null, true);
                echo "”</span>
      ";
            }
            // line 42
            echo "    ";
        }
        // line 43
        echo "  </ol>
</nav>
";
    }

    public function getTemplateName()
    {
        return "menu/breadcrumbs.twig";
    }

    public function isTraitable()
    {
        return false;
    }

    public function getDebugInfo()
    {
        return array (  149 => 43,  146 => 42,  140 => 40,  137 => 39,  131 => 37,  129 => 36,  121 => 32,  118 => 31,  115 => 30,  112 => 29,  109 => 28,  106 => 27,  103 => 26,  101 => 25,  97 => 24,  93 => 23,  90 => 22,  88 => 21,  80 => 17,  76 => 16,  72 => 15,  68 => 14,  65 => 13,  63 => 12,  55 => 8,  51 => 7,  47 => 6,  43 => 5,  37 => 1,);
    }

    public function getSourceContext()
    {
        return new Source("", "menu/breadcrumbs.twig", "C:\\OpenServer\\modules\\system\\html\\openserver\\phpmyadmin\\templates\\menu\\breadcrumbs.twig");
    }
}
