using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers.Scanners;

namespace ErrorRecoveryCombinators
{
    public class Terminal : ProductionBase<string>
    {
        private Token m_token;
        public Terminal(Token token)
        {
            m_token = token;
        }

        public override Parse<TFuture> BuildParse<TFuture>(Future<string, TFuture> future)
        {
            Parse<TFuture> parse = null;
            parse = scanner =>
            {
                // 保存读取当前单词之前的位置
                ForkableScanner prevScanner = scanner.Fork();

                Lexeme l = scanner.Read();

                if (l.TokenIndex == m_token.Index)
                {
                    return new StepResult<TFuture>(0, () => future(l.Value.Content)(scanner));
                }

                Lexeme recovery = l.GetErrorCorrectionLexeme(m_token.Index, m_token.Description);
                SyntaxError insertionErr = Grammar.RecoverByInsertion(recovery);

                if (l.IsEndOfStream)
                {
                    // 已经到了输入的末尾
                    // 插入预期的Token进行恢复
                    return new StepResult<TFuture>(1,
                        () => future(recovery.Value.Content)(prevScanner), insertionErr);
                }
                else
                {
                    // 同时尝试插入预期的Token，以及删除当前字符
                    // 在未来的解析中选取更好的路线
                    return Grammar.Best(
                        new StepResult<TFuture>(1, () => future(recovery.Value.Content)(prevScanner), insertionErr),
                        new StepResult<TFuture>(1, () => parse(scanner), Grammar.RecoverByDeletion(l)));
                }
            };

            return parse;
        }
    }
}
